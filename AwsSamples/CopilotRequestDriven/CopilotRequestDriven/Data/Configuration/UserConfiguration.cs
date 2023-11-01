using CopilotRequestDriven.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CopilotRequestDriven.Data.Configuration;

public class UserConfiguration:IEntityTypeConfiguration<User>
{
    // Guid.NewGuid();
    public const string Guid1 = "043c1a75-9f47-4629-b72e-f0a34c3eece6";
    private readonly PasswordHasher<User> _hasher = new();

    private readonly User _user1 = new()
    {
        Id = Guid1,
        UserName = "admin",
        NormalizedUserName = "admin".ToUpper(),
        Email = "dev@phasetr.com",
        NormalizedEmail = "dev@phasetr.com".ToUpper(),
        EmailConfirmed = true,
        SecurityStamp = string.Empty,
    };

    public void Configure(EntityTypeBuilder<User> builder)
    {
        _user1.PasswordHash = _hasher.HashPassword(_user1, "aspdotnetpass");
        builder.HasData(_user1);
    }
}
