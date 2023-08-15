using System.Security.Claims;
using System.Text;
using IdentityByRazorPages.Models.Contexts;
using IdentityByRazorPages.Models.Seeds;
using IdentityByRazorPages.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();

builder.Services.AddDbContext<IdContext>(opts =>
{
    opts.UseSqlite(builder.Configuration["ConnectionStrings:MyDbConnection"]);
    opts.EnableSensitiveDataLogging();
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IdContext>();

/*
builder.Services.Configure<IdentityOptions>(opts =>
{
    opts.Password.RequiredLength = 6;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireDigit = false;
    opts.User.RequireUniqueEmail = true;
    opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
});
*/

builder.Services.AddAuthentication(opts =>
{
    opts.DefaultScheme =
        CookieAuthenticationDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme =
        CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(opts =>
{
    opts.Events.DisableRedirectForPath(e => e.OnRedirectToLogin,
        "/api", StatusCodes.Status401Unauthorized);
    opts.Events.DisableRedirectForPath(e => e.OnRedirectToAccessDenied,
        "/api", StatusCodes.Status403Forbidden);
}).AddJwtBearer(opts =>
{
    opts.RequireHttpsMetadata = false;
    opts.SaveToken = true;
    opts.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(builder.Configuration["jwtSecret"])),
        ValidateAudience = false,
        ValidateIssuer = false
    };
    opts.Events = new JwtBearerEvents
    {
        OnTokenValidated = async ctx =>
        {
            var userMgr = ctx.HttpContext.RequestServices
                .GetRequiredService<UserManager<IdentityUser>>();
            var signinMgr = ctx.HttpContext.RequestServices
                .GetRequiredService<SignInManager<IdentityUser>>();
            var username =
                ctx.Principal?.FindFirst(ClaimTypes.Name)?.Value;
            var idUser = await userMgr.FindByNameAsync(username);
            ctx.Principal =
                await signinMgr.CreateUserPrincipalAsync(idUser);
        }
    };
});

var app = builder.Build();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute("controllers",
    "controllers/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.UseBlazorFrameworkFiles("/webassembly");
app.MapFallbackToFile("/webassembly/{*path:nonfile}", "/webassembly/index.html");

IdSeedData.EnsurePopulated(app, app.Configuration);
app.Run();
