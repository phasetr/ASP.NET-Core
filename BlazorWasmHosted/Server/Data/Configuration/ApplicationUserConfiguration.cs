using BlazorWasmHosted.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlazorWasmHosted.Server.Data.Configuration;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    private const string HashString = "st8OQllUM07ZsABC";
    private static readonly Guid Admin1Guid = Guid.Parse("b042e80a-386c-48c6-a5a4-3576ccf776db");
    private static readonly Guid Admin2Guid = Guid.Parse("967355e3-0b6e-46cb-8bb3-6cb0dd75b9f7");
    private static readonly PasswordHasher<string> Hasher = new();

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasIndex(u => u.UserName).IsUnique();
        builder.HasData(new ApplicationUser
        {
            Id = Admin1Guid.ToString(),
            UserName = "test1",
            NormalizedUserName = "test1".ToUpper(),
            Email = "test1@phasetr.com",
            NormalizedEmail = "test1@phasetr.com".ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = Admin1Guid.ToString(),
            ConcurrencyStamp = Admin1Guid.ToString(),
            PasswordHash = Hasher.HashPassword(HashString, "test1Test!")
        }, new ApplicationUser
        {
            Id = Admin2Guid.ToString(),
            UserName = "test2",
            NormalizedUserName = "test2".ToUpper(),
            Email = "test2@phasetr.com",
            NormalizedEmail = "test2@phasetr.com".ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = Admin2Guid.ToString(),
            ConcurrencyStamp = Admin2Guid.ToString(),
            PasswordHash = Hasher.HashPassword(HashString, "test2Test#")
        });
    }
}
