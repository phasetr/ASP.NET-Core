using MainSample.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MainSample.Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        var hasher = new PasswordHasher<User>();
        var user1 = new User
        {
            UserName = "Admin",
            Email = "example@phasetr.com",
            EmailConfirmed = true,
            SecurityStamp = string.Empty,
            FirstName = "First",
            LastName = "Last"
        };
        user1.PasswordHash = hasher.HashPassword(user1, "Secret123$");
        builder.HasData(user1);
    }
}