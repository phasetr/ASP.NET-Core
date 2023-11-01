using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CopilotLoadBalancedAspNet.Data.Configuration;

public class IdentityUserConfiguration : IEntityTypeConfiguration<IdentityUser>
{
    // F#, System.Guid.NewGuid()
    private static readonly Guid AdminGuid = Guid.Parse("f57cc766-ac94-4095-bcf7-6e2927e3c09f");
    private static readonly PasswordHasher<IdentityUser?> Hasher = new();

    public void Configure(EntityTypeBuilder<IdentityUser> builder)
    {
        builder.HasIndex(u => u.UserName).IsUnique();
        builder.HasData(new IdentityUser
        {
            Id = AdminGuid.ToString(),
            UserName = "dev@phasetr.com",
            NormalizedUserName = "dev@phasetr.com".ToUpper(),
            Email = "dev@phasetr.com",
            NormalizedEmail = "dev@phasetr.com".ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = AdminGuid.ToString(),
            ConcurrencyStamp = AdminGuid.ToString(),
            LockoutEnabled = true,
            PasswordHash = Hasher.HashPassword(null, "phasetrdevadmin")
        });
    }
}
