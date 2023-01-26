using MainSample.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MainSample.Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    // Guid.NewGuid();
    public static readonly string Guid1 = "043c1a75-9f47-4629-b72e-f0a34c3eece6";
    private readonly PasswordHasher<User> _hasher = new();

    private readonly User _user1 = new User
    {
        Id = Guid1,
        UserName = "Admin",
        NormalizedUserName = "Admin".ToUpper(),
        Email = "example@phasetr.com",
        NormalizedEmail = "example@phasetr.com".ToUpper(),
        EmailConfirmed = true,
        SecurityStamp = string.Empty,
        FirstName = "First",
        LastName = "Last"
    };
    
    public void Configure(EntityTypeBuilder<User> builder)
    {
        _user1.PasswordHash = _hasher.HashPassword(_user1, "Secret123$");
        builder.HasData(_user1);
    }
}