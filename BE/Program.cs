using Application.Common;
using Application.Practice;
using Application.System;
using AspNetCoreRateLimit;
using Data.Entities;
using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ZG04.Utilities;
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

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(Configuration.GetConnectionString(Constants.CONNECTION_STRING),
        options => options.EnableRetryOnFailure()
        );
    options.EnableDetailedErrors();

});

builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IQuestionSetService, QuestionSetService>();
builder.Services.AddScoped<IQuestionServices, QuestionService>();
builder.Services.AddScoped<ITestResultService, TestResultService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BE", Version = "v1" });
});
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>(); // Đăng ký IProcessingStrategy
builder.Services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>(); // Đăng ký IClientPolicyStore

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
        ValidIssuer = Configuration["Tokens:Issuer"],
        ValidateAudience = true,
        ValidAudience = Configuration["Tokens:Issuer"],
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),
        ClockSkew = TimeSpan.Zero
    };

    //cấu hình khi token hết hạn
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddMemoryCache();
builder.Services.Configure<ClientRateLimitOptions>(Configuration.GetSection("ClientRateLimiting"));
builder.Services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BE v1"));
}

app.UseStaticFiles();
app.UseClientRateLimiting();
app.UseIpRateLimiting();

string path = Path.Combine(Directory.GetCurrentDirectory(), FileService.FOLDER_NAME);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(path),
    RequestPath = FileService.REQUEST_PATH
});

app.UseCors("AllowAll");

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
