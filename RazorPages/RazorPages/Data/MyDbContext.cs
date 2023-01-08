using Microsoft.EntityFrameworkCore;
using RazorPages.Models;

namespace RazorPages.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movie { get; set; } = default!;
}