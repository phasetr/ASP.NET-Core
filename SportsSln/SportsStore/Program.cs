using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportsStore.Data;
using SportsStore.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IStoreRepository, EfStoreRepository>();
builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();

builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<IdDbContext>(options =>
    options.UseSqlite(builder.Configuration["ConnectionStrings:IdDbConnection"]));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IdDbContext>();

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

app.MapControllerRoute("catpage",
    "{category}/Page{productPage:int}",
    new {Controller = "Home", action = "Index"});

app.MapControllerRoute("page", "Page{productPage:int}",
    new {Controller = "Home", action = "Index", productPage = 1});

app.MapControllerRoute("category", "{category}",
    new {Controller = "Home", action = "Index", productPage = 1});

app.MapControllerRoute("pagination",
    "Products/Page{productPage}",
    new {Controller = "Home", action = "Index", productPage = 1});

app.MapDefaultControllerRoute();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");

IdentitySeedData.EnsurePopulated(app);

app.Run();