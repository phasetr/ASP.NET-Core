using BlazorJwtAuth.Common.EntityModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlazorJwtAuth.Common.DataContext.Data.Configuration;

public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
{
    public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
    {
        builder.HasData(new ApplicationUserRole
        {
            UserId = "userId",
            RoleId = "UserId"
        });
    }
}
