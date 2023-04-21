using BlazorWasmHosted.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlazorWasmHosted.Server.Data.Configuration;

public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
{
    public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
    {
        builder.HasData(new ApplicationUserRole
        {
            UserId = "1", RoleId = "1"
        }, new ApplicationUserRole
        {
            UserId = "2", RoleId = "2"
        });
    }
}
