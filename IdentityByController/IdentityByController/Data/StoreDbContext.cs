using IdentityByController.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityByController.Data;

public class StoreDbContext : DbContext
{
    public StoreDbContext(DbContextOptions<StoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
}