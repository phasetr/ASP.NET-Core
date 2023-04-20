using BlazorWasmHosted.Server.Data.Configuration;
using BlazorWasmHosted.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorWasmHosted.Server.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
    public DbSet<ApplicationRole> ApplicationRoles => Set<ApplicationRole>();
    public DbSet<Shop> Shops => Set<Shop>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder
            .ApplyConfiguration(new ApplicationUserConfiguration())
            .ApplyConfiguration(new ApplicationRoleConfiguration())
            .ApplyConfiguration(new ShopConfiguration());
    }
}
