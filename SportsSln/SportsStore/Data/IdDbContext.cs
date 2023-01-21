using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

namespace SportsStore.Data;

public class IdDbContext : IdentityDbContext<IdentityUser>
{
    public IdDbContext(DbContextOptions<IdDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
}