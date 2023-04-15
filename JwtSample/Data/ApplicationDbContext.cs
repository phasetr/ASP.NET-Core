using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data;

public class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ApplicationDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<ApiUser> ApiUsers { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // in memory database used for simplicity, change to a real db for production applications
        options.UseInMemoryDatabase("TestDb");
    }
}
