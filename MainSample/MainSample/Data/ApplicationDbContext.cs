using MainSample.Data.Configuration;
using MainSample.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MainSample.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<City> Cities { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<Pie> Pies { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Category>().HasData(new Category {CategoryId = 1, CategoryName = "Fruit pies"});
        builder.Entity<Pie>().HasData(new Pie
        {
            PieId = 1,
            CategoryId = 1,
            Name = "Apple Pie"
        });
        // builder
        //     .ApplyConfiguration(new CityConfiguration())
        //     .ApplyConfiguration(new CountryConfiguration())
        //     .ApplyConfiguration(new PropertyConfiguration())
        //     .ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new UserConfiguration());
        base.OnModelCreating(builder);
    }
}