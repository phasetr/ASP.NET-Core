using EFCoreQuestionSO20230315.Data.Configuration;
using EFCoreQuestionSO20230315.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EFCoreQuestionSO20230315.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUser => Set<ApplicationUser>();
    public DbSet<ApplicationRole> ApplicationRole => Set<ApplicationRole>();
    public DbSet<ApplicationUserRole> ApplicationUserRole => Set<ApplicationUserRole>();
    public DbSet<Book> Book => Set<Book>();
    public DbSet<Category> Category => Set<Category>();
    public DbSet<BookCategory> BookCategory => Set<BookCategory>();
    public DbSet<Shop> Shop => Set<Shop>();
    public DbSet<OrderNumber> OrderNumber => Set<OrderNumber>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder
            .ApplyConfiguration(new ApplicationUserConfiguration())
            .ApplyConfiguration(new ApplicationRoleConfiguration())
            .ApplyConfiguration(new ApplicationUserRoleConfiguration())
            .ApplyConfiguration(new BookConfiguration())
            .ApplyConfiguration(new CategoryConfiguration())
            .ApplyConfiguration(new BookCategoryConfiguration())
            .ApplyConfiguration(new ShopConfiguration())
            .ApplyConfiguration(new OrderNumberConfiguration());
    }
}
