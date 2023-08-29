using BlazorJwtAuth.Common.Constants;
using BlazorJwtAuth.Common.EntityModels.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlazorJwtAuth.Common.DataContext.Data.Configuration;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    private const string HashString = "dafldjalfjsafJAF";
    private static readonly PasswordHasher<string> Hasher = new();

    public static readonly ApplicationUser DefaultUser = new()
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
    }
}
