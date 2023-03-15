using EFCoreQuestionSO20230315.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreQuestionSO20230315.Data.Configuration;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public static readonly Guid AdminGuid = Guid.Parse("b965c7d9-4c34-48e4-ae8d-da4ff2528bb7");
    public static readonly Guid CustomerGuid = Guid.Parse("661e47ac-eb60-45da-822d-f15498678638");
    public static readonly Guid StaffGuid = Guid.Parse("7bf97804-9e5c-4fda-9119-0f91ede6d2e3");

    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData(new ApplicationRole {Id = AdminGuid.ToString(), Name = "Admin"});
        builder.HasData(new ApplicationRole {Id = StaffGuid.ToString(), Name = "Staff"});
        builder.HasData(new ApplicationRole {Id = CustomerGuid.ToString(), Name = "Customer"});
    }
}
