using Common.Constants;
using Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataContext.Data.Configuration;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    private const string HashString = "dafldjalfjsafJAF";
    private static readonly PasswordHasher<string> Hasher = new();

    private static readonly ApplicationUser DefaultUser = new()
    {
        Id = $"{Authorization.DefaultUsername}Id",
        UserName = Authorization.DefaultUsername,
        NormalizedUserName = Authorization.DefaultUsername.ToUpper(),
        Email = Authorization.DefaultEmail,
        NormalizedEmail = Authorization.DefaultEmail.ToUpper(),
        EmailConfirmed = true,
        FirstName = "First",
        LastName = "Last",
        PhoneNumberConfirmed = true,
        PasswordHash = Hasher.HashPassword(HashString, Authorization.DefaultPassword)
    };

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasData(DefaultUser);
        builder.HasData(new ApplicationUser
        {
            Id = "adminId",
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@phasetr.com",
            NormalizedEmail = "ADMIN@PHASETR.COM",
            EmailConfirmed = true,
            FirstName = "admin first",
            LastName = "admin last",
            PhoneNumberConfirmed = true,
            PasswordHash = Hasher.HashPassword(HashString, "adminpass")
        });
    }
}
