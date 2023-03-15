using EFCoreQuestionSO20230315.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreQuestionSO20230315.Data.Configuration;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public static readonly Guid Admin1Guid = Guid.Parse("043c1a75-9f47-4629-b72e-f0a34c3eece6");
    public static readonly Guid Staff1Guid = Guid.Parse("9a5a81b3-1a2d-491e-8414-873bf8dca382");
    public static readonly Guid Customer1Guid = Guid.Parse("fbccfc79-af93-4da4-9213-67a6bd333ce1");

    private readonly PasswordHasher<ApplicationUser> _hasher = new();

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasData(new ApplicationUser
        {
            Id = Admin1Guid.ToString(),
            UserName = "admin",
            NormalizedUserName = "admin".ToUpper(),
            Email = "dev@phasetr.com",
            NormalizedEmail = "dev@phasetr.com".ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString(),
            PasswordHash = _hasher.HashPassword(new ApplicationUser(), "phasetrdevadmin")
        });
        builder.HasData(new ApplicationUser
        {
            Id = Staff1Guid.ToString(),
            UserName = "staff",
            NormalizedUserName = "staff".ToUpper(),
            Email = "dev@phasetr.com",
            NormalizedEmail = "dev@phasetr.com".ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString(),
            PasswordHash = _hasher.HashPassword(new ApplicationUser(), "phasetrdevstaff")
        });
        builder.HasData(new ApplicationUser
        {
            Id = Customer1Guid.ToString(),
            UserName = "customer",
            NormalizedUserName = "customer".ToUpper(),
            Email = "dev@phasetr.com",
            NormalizedEmail = "dev@phasetr.com".ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString(),
            PasswordHash = _hasher.HashPassword(new ApplicationUser(), "phasetrdevcustomer")
        });
    }
}
