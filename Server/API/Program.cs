
using API.Middlewares;
using AspNetCoreRateLimit;
using Core.Common;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices.Common;
using Core.Interfaces.IServices.Features;
using Core.Interfaces.IServices.Management;
using Core.Interfaces.IServices.System;
using Core.RealTime;
using Core.Repositories;
using Core.Services.Common;
using Core.Services.Features;
using Core.Services.Management;
using Core.Services.System;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection"),
        options => options.EnableRetryOnFailure());
    options.EnableDetailedErrors();
});

builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddTransient<IRoleManagementService, RoleManagementService>();
builder.Services.AddTransient<IUserManagementService, UserManagementService>();
builder.Services.AddTransient<ITestManagementService, TestManagementService>();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ITestRepository, TestRepository>();
builder.Services.AddTransient<ICommentRepository, CommentRepository>();
builder.Services.AddTransient<ISummaryRepository, SummaryRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();

builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<IFileService, FileService>();
builder.Services.AddSingleton<ILogService, LogService>();

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ITestService, TestService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ICommentService, CommentService>();
builder.Services.AddTransient<IHomeService, HomeService>();
builder.Services.AddSingleton<ISummaryService, SummaryService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<TrackingMiddleware, TrackingMiddleware>();


builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BE", Version = "v1" });
});
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>(); // Đăng ký IProcessingStrategy
builder.Services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>(); // Đăng ký IClientPolicyStore

//cấu hình login
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
    //pass config
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
});

//cấu hình xác thực
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration.GetSection(Consts.JWT_SECTION)["Issuer"],
        ValidAudience = configuration.GetSection(Consts.JWT_SECTION)["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(Consts.SECRET_KEY))),
        ClockSkew = TimeSpan.Zero
    };

    //cấu hình khi token hết hạn
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                //context.Response.Headers.AddRole("Token-Expired", "true");
                context.Request.HttpContext.User = null;
            }
            return Task.CompletedTask;
        }
    };
});


//cấu hình giới hạn truy cập
builder.Services.AddMemoryCache();
builder.Services.Configure<ClientRateLimitOptions>(configuration.GetSection("ClientRateLimiting"));
builder.Services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));

//cấu hình cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient",
        builder => builder
            .WithOrigins("http://localhost:4200")
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

//logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
builder.Host.UseSerilog();

//singalR
builder.Services.AddSignalR();

var app = builder.Build();

//cors
app.UseCors("AllowClient");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BE v1"));
}



app.UseMiddleware<HandleExceptionMiddleware>();
app.UseMiddleware<TrackingMiddleware>();


//app.UseClientRateLimiting();
//app.UseIpRateLimiting();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(AppContext.BaseDirectory, "Resources", "Images", "System")),
    RequestPath = "/api/images/system"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(AppContext.BaseDirectory, "Resources", "Images", "Test")),
    RequestPath = "/api/images/test"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(AppContext.BaseDirectory, "Resources", "Images", "User")),
    RequestPath = "/api/images/user"
});

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

//singalR
app.MapHub<LogHub>("/logHub");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    SeedData.Initialize(roleManager, userManager, dbContext).Wait();
}

app.Run();
