using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models;

public static class IdentitySeedData
{
    private const string adminUser = "Admin";
    private const string adminPassword = "Secret123$";

    public static async void EnsurePopulated(IApplicationBuilder app)
    {
        var context = app.ApplicationServices
            .CreateScope().ServiceProvider
            .GetRequiredService<AppIdentityDbContext>();
        if (context.Database.GetPendingMigrations().Any()) context.Database.Migrate();

        var userManager = app.ApplicationServices
            .CreateScope().ServiceProvider
            .GetRequiredService<UserManager<IdentityUser>>();

        var user = await userManager.FindByNameAsync(adminUser);
        if (user == null)
        {
            user = new IdentityUser("Admin");
            user.Email = "admin@example.com";
            user.PhoneNumber = "555-1234";
            await userManager.CreateAsync(user, adminPassword);
        }
    }
}