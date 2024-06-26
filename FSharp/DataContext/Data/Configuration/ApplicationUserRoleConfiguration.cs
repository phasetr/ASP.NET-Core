using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataContext.Data.Configuration;

public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
{
    public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
    {
        builder.HasData(new ApplicationUserRole
        {
            UserId = "userId",
            RoleId = "UserId"
        });
        builder.HasData(new ApplicationUserRole
        {
            UserId = "adminId",
            RoleId = "AdministratorId"
        });
    }
}
