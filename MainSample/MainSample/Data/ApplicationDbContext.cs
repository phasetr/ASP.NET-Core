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
    public DbSet<PieCategory> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder
            .ApplyConfiguration(new CountryConfiguration())
            .ApplyConfiguration(new CityConfiguration())
            //.ApplyConfiguration(new PropertyConfiguration())
            .ApplyConfiguration(new PieCategoryConfiguration())
            .ApplyConfiguration(new PieConfiguration())
            .ApplyConfiguration(new UserConfiguration());
        base.OnModelCreating(builder);
    }
}