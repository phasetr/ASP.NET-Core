using Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Models;

public class SeedDataIdentity
{
    private const string AdminUser = "Admin";
    private const string AdminPassword = "Secret123$";

    public static async void EnsurePopulated(IApplicationBuilder app)
    {
        var context = app.ApplicationServices
            .CreateScope().ServiceProvider
            .GetRequiredService<ApplicationDbContext>();
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
    }
}