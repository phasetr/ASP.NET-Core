using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportsStore.Data;

namespace SportsStore.Models;

public static class IdentitySeedData
{
    private const string AdminUser = "Admin";
    private const string AdminPassword = "Secret123$";

    public static async void EnsurePopulated(IApplicationBuilder app)
    {
        var context = app.ApplicationServices
            .CreateScope().ServiceProvider
            .GetRequiredService<IdDbContext>();
        if ((await context.Database.GetPendingMigrationsAsync()).Any()) await context.Database.MigrateAsync();

        var userManager = app.ApplicationServices
            .CreateScope().ServiceProvider
            .GetRequiredService<UserManager<IdentityUser>>();

        var user = await userManager.FindByNameAsync(AdminUser);
        if (user != null) return;
        user = new IdentityUser("Admin")
        {
            Email = "admin@example.com",
            PhoneNumber = "555-1234"
        };
        await userManager.CreateAsync(user, AdminPassword);
        
        if ((await context.Database.GetPendingMigrationsAsync()).Any()) await context.Database.MigrateAsync();

        if (context.Products.Any()) return;
        context.Products.AddRange(
            new Product
            {
                Name = "Kayak", Description = "A boat for one person",
                Category = "WaterSports", Price = 275
            },
            new Product
            {
                Name = "LifeJacket",
                Description = "Protective and fashionable",
                Category = "WaterSports", Price = 48.95m
            },
            new Product
            {
                Name = "Soccer Ball",
                Description = "FIFA-approved size and weight",
                Category = "Soccer", Price = 19.50m
            },
            new Product
            {
                Name = "Corner Flags",
                Description = "Give your playing field a professional touch",
                Category = "Soccer", Price = 34.95m
            },
            new Product
            {
                Name = "Stadium",
                Description = "Flat-packed 35,000-seat stadium",
                Category = "Soccer", Price = 79500
            },
            new Product
            {
                Name = "Thinking Cap",
                Description = "Improve brain efficiency by 75%",
                Category = "Chess", Price = 16
            },
            new Product
            {
                Name = "Unsteady Chair",
                Description = "Secretly give your opponent a disadvantage",
                Category = "Chess", Price = 29.95m
            },
            new Product
            {
                Name = "Human Chess Board",
                Description = "A fun game for the family",
                Category = "Chess", Price = 75
            },
            new Product
            {
                Name = "Bling-Bling King",
                Description = "Gold-plated, diamond-studded King",
                Category = "Chess", Price = 1200
            }
        );
        await context.SaveChangesAsync();
    }
}