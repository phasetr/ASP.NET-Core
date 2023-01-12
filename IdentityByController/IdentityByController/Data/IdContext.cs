using IdentityByController.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityByController.Data;

public class IdContext : IdentityDbContext<IdentityUser>
{
    public IdContext(DbContextOptions<IdContext> options)
        : base(options)
    {
    }
    
    public DbSet<Product> Products => Set<Product>();
}