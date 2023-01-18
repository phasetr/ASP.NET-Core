using CityBreaks.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityBreaks.Data.Configuration;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.Property(x => x.CountryName)
            .HasMaxLength(50);
        builder.Property(x => x.CountryCode)
            .HasColumnName("ISO 3166 code")
            .HasMaxLength(2);
        builder.HasData(new List<Country>
        {
            new() {Id = 1, CountryName = "Croatia", CountryCode = "hr"},
            new() {Id = 2, CountryName = "Denmark", CountryCode = "dk"},
            new() {Id = 3, CountryName = "France", CountryCode = "fr"},
            new() {Id = 4, CountryName = "Germany", CountryCode = "de"},
            new() {Id = 5, CountryName = "Holland", CountryCode = "nl"},
            new() {Id = 6, CountryName = "Italy", CountryCode = "it"},
            new() {Id = 7, CountryName = "Spain", CountryCode = "es"},
            new() {Id = 8, CountryName = "United Kingdom", CountryCode = "gb"},
            new() {Id = 9, CountryName = "United States", CountryCode = "us"}
        });
    }
}