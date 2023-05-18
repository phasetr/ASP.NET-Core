using EFCoreQuestionSO20230315.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreQuestionSO20230315.Data.Configuration;

public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
{
    public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
    {
        builder.HasData(new ApplicationUserRole
        {
            UserId = ApplicationUserConfiguration.Admin1Guid.ToString(),
            RoleId = ApplicationRoleConfiguration.AdminGuid.ToString()
        });
        builder.HasData(new ApplicationUserRole
        {
            UserId = ApplicationUserConfiguration.Staff1Guid.ToString(),
            RoleId = ApplicationRoleConfiguration.StaffGuid.ToString()
        });
        builder.HasData(new ApplicationUserRole
        {
            UserId = ApplicationUserConfiguration.Customer1Guid.ToString(),
            RoleId = ApplicationRoleConfiguration.CustomerGuid.ToString()
        });
    }
}
