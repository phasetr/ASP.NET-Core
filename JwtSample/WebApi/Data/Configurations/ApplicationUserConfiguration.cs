using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models;

namespace WebApi.Data.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    private const string HashString = "st8OQllUM07ZsLSJ";

    // System.Guid.NewGuid().ToString()
    private static readonly Guid Admin1Guid = Guid.Parse("9145cd60-8280-428e-a399-abfbfe7db5a9");
    private static readonly PasswordHasher<string> Hasher = new();

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasIndex(u => u.UserName).IsUnique();
        builder.HasData(new ApplicationUser
        {
            Id = Admin1Guid.ToString(),
            UserName = "test",
            FirstName = "test first",
            LastName = "test last",
            NormalizedUserName = "test".ToUpper(),
            Email = "test@phasetr.com",
            NormalizedEmail = "test@phasetr.com".ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = Admin1Guid.ToString(),
            ConcurrencyStamp = Admin1Guid.ToString(),
            PasswordHash = Hasher.HashPassword(HashString, "testTest")
        });
    }
}
