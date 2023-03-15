using EFCoreQuestionSO20230315.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreQuestionSO20230315.Data.Configuration;

public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
{
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<ApplicationRole> ROles { get; set; }
    public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
    {
        builder.HasKey(m => new {m.UserId, m.RoleId});
        builder.HasData(new ApplicationUserRole
            {UserId = ApplicationUserConfiguration.Admin1Guid, RoleId = ApplicationRoleConfiguration.AdminGuid});
        builder.HasData(new ApplicationUserRole
            {UserId = ApplicationUserConfiguration.Staff1Guid, RoleId = ApplicationRoleConfiguration.StaffGuid});
        builder.HasData(new ApplicationUserRole
            {UserId = ApplicationUserConfiguration.Customer1Guid, RoleId = ApplicationRoleConfiguration.CustomerGuid});
    }
}
