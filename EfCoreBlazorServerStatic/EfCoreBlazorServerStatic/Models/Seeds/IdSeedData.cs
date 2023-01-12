using EfCoreBlazorServerStatic.Models.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EfCoreBlazorServerStatic.Models.Seeds;

public static class IdSeedData
{
    public static async void SetSeeds(IApplicationBuilder app, IConfiguration configuration)
    {
        var serviceProvider = app.ApplicationServices.CreateScope().ServiceProvider;
        var context = serviceProvider.GetRequiredService<IdContext>();

        var userManager =
            serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager =
            serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var username = configuration["Data:AdminUser:Name"] ?? "Admin";
        var email
            = configuration["Data:AdminUser:Email"] ?? "admin@example.com";
        var password = configuration["Data:AdminUser:Password"] ?? "Secret123$";
        var role = configuration["Data:AdminUser:Role"] ?? "Admins";

        if (await roleManager.FindByNameAsync(role) == null) await roleManager.CreateAsync(new IdentityRole(role));
        if (await userManager.FindByNameAsync(username) == null)
        {
            var user = new ApplicationUser
            {
                UserName = username,
                Email = email
            };

            var result = await userManager
                .CreateAsync(user, password);
            if (result.Succeeded) await userManager.AddToRoleAsync(user, role);

            await context.Database.MigrateAsync();
            if (context.People.Any() || context.Departments.Any() ||
                context.Locations.Any() || context.Articles.Any()) return;
            var d1 = new Department {Name = "Sales"};
            var d2 = new Department {Name = "Development"};
            var d3 = new Department {Name = "Support"};
            var d4 = new Department {Name = "Facilities"};

            context.Departments.AddRange(d1, d2, d3, d4);
            await context.SaveChangesAsync();

            var l1 = new Location {City = "Oakland", State = "CA"};
            var l2 = new Location {City = "San Jose", State = "CA"};
            var l3 = new Location {City = "New York", State = "NY"};
            context.Locations.AddRange(l1, l2, l3);

            var u1 = await context.ApplicationUsers.FirstAsync();
            var a1 = new Article {Title = "Title 1", Description = "Description 1", User = u1};
            var a2 = new Article {Title = "Title 2", Description = "Description 2", User = u1};
            context.Articles.AddRange(a1, a2);

            context.People.AddRange(
                new Person
                {
                    Firstname = "Francesca", Surname = "Jacobs",
                    Department = d2, Location = l1
                },
                new Person
                {
                    Firstname = "Charles", Surname = "Fuentes",
                    Department = d2, Location = l3
                },
                new Person
                {
                    Firstname = "Bright", Surname = "Becker",
                    Department = d4, Location = l1
                },
                new Person
                {
                    Firstname = "Murphy", Surname = "Lara",
                    Department = d1, Location = l3
                },
                new Person
                {
                    Firstname = "Beasley", Surname = "Hoffman",
                    Department = d4, Location = l3
                },
                new Person
                {
                    Firstname = "Marks", Surname = "Hays",
                    Department = d4, Location = l1
                },
                new Person
                {
                    Firstname = "Underwood", Surname = "Trujillo",
                    Department = d2, Location = l1
                },
                new Person
                {
                    Firstname = "Randall", Surname = "Lloyd",
                    Department = d3, Location = l2
                },
                new Person
                {
                    Firstname = "Guzman", Surname = "Case",
                    Department = d2, Location = l2
                });
            await context.SaveChangesAsync();
        }
    }
}