using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models;

namespace WebApi.Data.Configuration;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    private const string HashString = "st8OQllUM07ZsLSJ";
    private static readonly PasswordHasher<string> Hasher = new();

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasIndex(u => u.UserName).IsUnique();
        builder.HasData(new ApplicationUser
        {
            Id = "admin",
            UserName = "adminName",
            NormalizedUserName = "admin".ToUpper(),
            Email = "dev@phasetr.com",
            NormalizedEmail = "dev@phasetr.com".ToUpper(),
            EmailConfirmed = true,
            PasswordHash = Hasher.HashPassword(HashString, "admin")
        });
        builder.HasData(new ApplicationUser
        {
            Id = "user1",
            UserName = "user1Name",
            NormalizedUserName = "user1Name".ToUpper(),
            Email = "user1@phasetr.com",
            NormalizedEmail = "user1@phasetr.com".ToUpper(),
            EmailConfirmed = true,
            PasswordHash = Hasher.HashPassword(HashString, "user1")
        });
    }
}
