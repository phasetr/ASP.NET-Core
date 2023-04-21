using BlazorWasmHosted.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlazorWasmHosted.Server.Data.Configuration;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    private const string HashString = "st8OQllUM07ZsABC";
    private static readonly PasswordHasher<string> Hasher = new();

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasIndex(u => u.UserName).IsUnique();
        builder.HasData(new ApplicationUser
        {
            Id = "1",
            UserName = "test1",
            NormalizedUserName = "test1".ToUpper(),
            Email = "test1@phasetr.com",
            NormalizedEmail = "test1@phasetr.com".ToUpper(),
            EmailConfirmed = true,
            PasswordHash = Hasher.HashPassword(HashString, "test1Test!")
        }, new ApplicationUser
        {
            Id = "2",
            UserName = "test2",
            NormalizedUserName = "test2".ToUpper(),
            Email = "test2@phasetr.com",
            NormalizedEmail = "test2@phasetr.com".ToUpper(),
            EmailConfirmed = true,
            PasswordHash = Hasher.HashPassword(HashString, "test2Test#")
        });
    }
}
