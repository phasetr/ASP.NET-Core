using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EfCoreBlazorServerStatic.Models.Contexts;

public class IdContext : IdentityDbContext<IdentityUser>
{
    public IdContext(DbContextOptions<IdContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<Person> People => Set<Person>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Location> Locations => Set<Location>();
}