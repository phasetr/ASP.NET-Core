using Common.Constants;
using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataContext.Data.Configuration;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData(
            new ApplicationRole
            {
                Id = $"{Authorization.Roles.Administrator}Id",
                Name = Authorization.Roles.Administrator.ToString(),
                NormalizedName = Authorization.Roles.Administrator.ToString().ToUpper()
            },
            new ApplicationRole
            {
                Id = $"{Authorization.Roles.Moderator}Id",
                Name = Authorization.Roles.Moderator.ToString(),
                NormalizedName = Authorization.Roles.Moderator.ToString().ToUpper()
            },
            new ApplicationRole
            {
                Id = $"{Authorization.Roles.User}Id",
                Name = Authorization.Roles.User.ToString(),
                NormalizedName = Authorization.Roles.User.ToString().ToUpper()
            });
    }
}
