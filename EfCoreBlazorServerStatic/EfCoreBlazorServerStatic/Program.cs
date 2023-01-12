using EfCoreBlazorServerStatic.Models.Contexts;
using EfCoreBlazorServerStatic.Models.Seeds;
using EfCoreBlazorServerStatic.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();

builder.Services.AddDbContext<IdContext>(opts =>
{
    opts.UseSqlite(builder.Configuration[
        "ConnectionStrings:IdentityConnection"]);
    opts.EnableSensitiveDataLogging();
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IdContext>();

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
});

var app = builder.Build();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapFallbackToPage("/_Host");

app.UseBlazorFrameworkFiles("/webassembly");
app.MapFallbackToFile("/webassembly/{*path:nonfile}", "/webassembly/index.html");

var context = app.Services.CreateScope().ServiceProvider
    .GetRequiredService<IdContext>();
IdSeedData.SetSeeds(app, app.Configuration);
app.Run();