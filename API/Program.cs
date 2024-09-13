using Application.Common;
using AspNetCoreRateLimit;
using Data.Entities;
using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.Extensions.Hosting;
using Utilities;
using Application.System.Users;
using Application.System.Roles;
using Application.System.Auth;
using Application.System.Manage;
using Application.Features.Learn;
using System.Web.Http.Results;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        Configuration.GetConnectionString(Consts.CONNECTION_STRING),
        options => options.EnableRetryOnFailure());
    options.EnableDetailedErrors();
});

builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<IRoleService, RoleService>();

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
        ValidIssuer = Configuration[Consts.AppSettingsKey.ISSUER],
        ValidAudience = Configuration[Consts.AppSettingsKey.AUDIENCE],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(Consts.EnvKey.SECRET_KEY))),
        ClockSkew = TimeSpan.Zero
    };

    //cấu hình khi token hết hạn
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                //context.Response.Headers.Add("Token-Expired", "true");
                context.Request.HttpContext.User = null;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole(Consts.DEFAULT_ADMIN_ROLE));
});

//cấu hình cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
    });
});


//cấu hình giới hạn truy cập
builder.Services.AddMemoryCache();
builder.Services.Configure<ClientRateLimitOptions>(Configuration.GetSection("ClientRateLimiting"));
builder.Services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BE v1"));
}

app.UseCors("AllowSpecificOrigin");
//app.UseClientRateLimiting();
//app.UseIpRateLimiting();

string path = Path.Combine(Directory.GetCurrentDirectory(), FileService.FOLDER_NAME);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(path),
    RequestPath = FileService.REQUEST_PATH
});

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var services = scope.ServiceProvider;
    SeedData.Initialize(services, userManager).Wait();
}

app.Run();
