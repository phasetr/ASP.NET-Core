using EFCoreQuestionSO20230315.Data.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EFCoreQuestionSO20230315.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

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
