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

    public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
    public DbSet<ApplicationRole> ApplicationRoles => Set<ApplicationRole>();
    public DbSet<ApplicationUserRole> ApplicationUserRoles => Set<ApplicationUserRole>();
    public DbSet<ApplicationUserShop> ApplicationUserShops => Set<ApplicationUserShop>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<BookCategory> BookCategories => Set<BookCategory>();
    public DbSet<Shop> Shops => Set<Shop>();
    public DbSet<OrderNumber> OrderNumbers => Set<OrderNumber>();
    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
    public DbSet<Product> Products => Set<Product>();

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
            .ApplyConfiguration(new OrderNumberConfiguration())
            .ApplyConfiguration(new PaymentMethodConfiguration())
            .ApplyConfiguration(new ApplicationUserShopConfiguration())
            .ApplyConfiguration(new ProductConfiguration());
    }
}
