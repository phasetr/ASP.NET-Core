using IdentityByController.Data;
using IdentityByController.Models;
using IdentityByController.Models.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<IdContext>(opts =>
{
    opts.UseSqlite(builder.Configuration["ConnectionStrings:MyDbContext"]);
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IdContext>();

builder.Services.AddScoped<IStoreRepository, EfStoreRepository>();

builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

if (app.Environment.IsProduction()) app.UseExceptionHandler("/error");

app.UseRequestLocalization(opts =>
{
    opts.AddSupportedCultures("en-US")
        .AddSupportedUICultures("en-US")
        .SetDefaultCulture("en-US");
});

app.UseStaticFiles();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute("page", "Page{productPage:int}",
    new {Controller = "Home", action = "Index", productPage = 1});

app.MapControllerRoute("pagination",
    "Products/Page{productPage}",
    new {Controller = "Home", action = "Index", productPage = 1});

app.MapDefaultControllerRoute();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");

IdSeedData.EnsurePopulated(app);

app.Run();