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

    public DbSet<ApplicationUser>? ApplicationUser { get; set; }
    public DbSet<ApplicationRole>? ApplicationRole { get; set; }
    public DbSet<ApplicationUserRole>? ApplicationUserRole { get; set; }
    public DbSet<Book>? Book { get; set; }
    public DbSet<Category>? Category { get; set; }
    public DbSet<BookCategory>? BookCategory { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder
            .ApplyConfiguration(new ApplicationUserConfiguration())
            .ApplyConfiguration(new ApplicationRoleConfiguration())
            .ApplyConfiguration(new ApplicationUserRoleConfiguration())
            .ApplyConfiguration(new BookConfiguration())
            .ApplyConfiguration(new CategoryConfiguration())
            .ApplyConfiguration(new BookCategoryConfiguration());
    }
}
