using EFCoreQuestionSO20230315.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreQuestionSO20230315.Data.Configuration;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public static readonly Guid AdminGuid = Guid.Parse("b965c7d9-4c34-48e4-ae8d-da4ff2528bb7");
    public static readonly Guid StaffGuid = Guid.Parse("7bf97804-9e5c-4fda-9119-0f91ede6d2e3");
    public static readonly Guid CustomerGuid = Guid.Parse("661e47ac-eb60-45da-822d-f15498678638");

    public static readonly ApplicationRole AdminRole = new()
        {Id = AdminGuid.ToString(), Name = "Admin", NormalizedName = "Admin".ToUpper()};

    public static readonly ApplicationRole StaffRole = new()
        {Id = StaffGuid.ToString(), Name = "Staff", NormalizedName = "Staff".ToUpper()};

    public static readonly ApplicationRole CustomerRole = new()
        {Id = CustomerGuid.ToString(), Name = "Customer", NormalizedName = "Customer".ToUpper()};

    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData(AdminRole, StaffRole, CustomerRole);
    }
}
