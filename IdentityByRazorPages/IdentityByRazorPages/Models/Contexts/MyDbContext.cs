using Microsoft.EntityFrameworkCore;

namespace IdentityByRazorPages.Models.Contexts;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> opts)
        : base(opts)
    {
    }

    public DbSet<Person> People => Set<Person>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Location> Locations => Set<Location>();
}