using Microsoft.EntityFrameworkCore;
using MvcWithApi.Models;

namespace MvcWithApi.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movie { get; set; } = default!;
}