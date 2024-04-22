using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api;

public static class SeedData
{
    private static readonly IEnumerable<SeedUser> SeedUsers =
    [
        new SeedUser
        {
            Email = "leela@contoso.com",
            NormalizedEmail = "LEELA@CONTOSO.COM",
            NormalizedUserName = "LEELA@CONTOSO.COM",
            RoleList = ["Administrator", "Manager"],
            UserName = "leela@contoso.com"
        },
        new SeedUser
        {
            Email = "harry@contoso.com",
            NormalizedEmail = "HARRY@CONTOSO.COM",
            NormalizedUserName = "HARRY@CONTOSO.COM",
            RoleList = ["User"],
            UserName = "harry@contoso.com"
        }
    ];

    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        await using var context =
            new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

        if (context.Users.Any()) return;

        var userStore = new UserStore<ApplicationUser>(context);
        var password = new PasswordHasher<ApplicationUser>();

        using var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        string[] roles = ["Administrator", "Manager", "User"];

        foreach (var role in roles)
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = role
                });

        using var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var user in SeedUsers)
        {
            var hashed = password.HashPassword(user, "Passw0rd!");
            user.PasswordHash = hashed;
            await userStore.CreateAsync(user);

            if (user.Email is null) continue;
            var appUser = await userManager.FindByEmailAsync(user.Email);
            if (appUser is not null && user.RoleList is not null)
                await userManager.AddToRolesAsync(appUser, user.RoleList);
        }

        await context.SaveChangesAsync();
    }

    private class SeedUser : ApplicationUser
    {
        public string[]? RoleList { get; init; }
    }
}
