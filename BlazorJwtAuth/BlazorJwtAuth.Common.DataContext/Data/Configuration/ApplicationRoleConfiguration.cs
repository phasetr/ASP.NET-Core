using BlazorJwtAuth.Common.Constants;
using BlazorJwtAuth.Common.EntityModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlazorJwtAuth.Common.DataContext.Data.Configuration;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData(
            new ApplicationRole
            {
                Id = $"{Authorization.Roles.Administrator.ToString()}Id",
                Name = Authorization.Roles.Administrator.ToString(),
                NormalizedName = Authorization.Roles.Administrator.ToString().ToUpper()
            },
            new ApplicationRole
            {
                Id = $"{Authorization.Roles.Moderator.ToString()}Id",
                Name = Authorization.Roles.Moderator.ToString(),
                NormalizedName = Authorization.Roles.Moderator.ToString().ToUpper()
            },
            new ApplicationRole
            {
                Id = $"{Authorization.Roles.User.ToString()}Id",
                Name = Authorization.Roles.User.ToString(),
                NormalizedName = Authorization.Roles.User.ToString().ToUpper()
            });
    }
}
