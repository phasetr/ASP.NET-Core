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
            Id = 1, Name = "Admin", NormalizedName = "Admin".ToUpper()
        }, new ApplicationRole
        {
            Id = 2, Name = "Staff", NormalizedName = "Staff".ToUpper()
        }, new ApplicationRole
        {
            Id = 3, Name = "Customer", NormalizedName = "Customer".ToUpper()
        });
    }
}
