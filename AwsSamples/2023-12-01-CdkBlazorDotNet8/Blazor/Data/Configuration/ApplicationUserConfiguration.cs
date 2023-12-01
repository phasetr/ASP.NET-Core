using Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blazor.Data.Configuration;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    private const string HashString = "dafldjalfjsafJAF";
    private static readonly PasswordHasher<string> Hasher = new();

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // TODO: Add DefaultUser to the database
    }
}
