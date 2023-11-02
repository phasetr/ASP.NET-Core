using System;
using System.Text;
using System.Threading.Tasks;
using Common.Constants;
using Common.DataContext.Data;
using Common.Entities;
using Common.Services;
using Common.Services.Interfaces;
using Common.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Service.Services;
using Service.Services.Interfaces;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);
// Configuration from AppSettings
builder.Services.Configure<Jwt>(builder.Configuration.GetSection("Jwt"));

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("WebApi")));

// Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
});
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
        options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<ApplicationRole>()
    .AddRoleManager<RoleManager<ApplicationRole>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.WithOrigins("https://localhost:7107");
        corsPolicyBuilder.AllowAnyMethod();
        corsPolicyBuilder.AllowAnyHeader();
        corsPolicyBuilder.AllowCredentials();
    });
});

// Controllers
builder.Services.AddControllers();
// `Minimal API`の`Swagger`ドキュメント生成用
builder.Services.AddEndpointsApiExplorer();
// Swagger
builder.Services.AddSwaggerGen();

// DI設定
builder.Services.AddScoped<IApplicationRoleService, ApplicationRoleService>();
builder.Services.AddScoped<IClaimsService, ClaimsService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IPtDateTime, PtDateTime>();
builder.Services.AddSingleton<Random>();
builder.Services.AddScoped<IUserService, UserService>();

// Cookie：特にJWT認証・CSRF用
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = _ => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.HttpOnly = HttpOnlyPolicy.Always;
});

// Adding Authentication - JWT
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    // JWTトークンをクッキーに書き込むための設定
    .AddCookie(options =>
    {
        options.Cookie.Name = Authorization.JwtAccessTokenName;
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    })
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = true;
        o.SaveToken = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
        // Cookieからトークンを取得するための設定
        o.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey(Authorization.JwtAccessTokenName))
                    context.Token = context.Request.Cookies[Authorization.JwtAccessTokenName];
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// 開発用の設定：開発用例外ページ・SwaggerUI
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 全体的なエラーハンドリング：500設定
app.UseMiddleware<CustomErrorHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.Run();

// 統合テストのために追加
// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program
{
}
