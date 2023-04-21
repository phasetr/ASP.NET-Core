using BlazorWasmHosted.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlazorWasmHosted.Server.Data.Configuration;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData(new ApplicationRole
        {
            Id = "1", Name = nameof(UserRoles.Admin), NormalizedName = nameof(UserRoles.Admin).ToUpper()
        }, new ApplicationRole
        {
            Id = "2", Name = nameof(UserRoles.User), NormalizedName = nameof(UserRoles.User).ToUpper()
        });
    }
}
