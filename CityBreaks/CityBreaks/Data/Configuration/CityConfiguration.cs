using CityBreaks.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityBreaks.Data.Configuration;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.Property(x => x.Photo).HasColumnName("Image");
        builder.HasData(new List<City>
        {
            new() {Id = 1, Name = "Amsterdam", CountryId = 5, Photo = "amsterdam.jpg"},
            new() {Id = 2, Name = "Barcelona", CountryId = 7, Photo = "barcelona.jpg"},
            new() {Id = 3, Name = "Berlin", CountryId = 4, Photo = "berlin.jpg"},
            new() {Id = 4, Name = "Copenhagen", CountryId = 2, Photo = "copenhagen.jpg"},
            new() {Id = 5, Name = "Dubrovnik", CountryId = 1, Photo = "dubrovnik.jpg"},
            new() {Id = 6, Name = "Edinburgh", CountryId = 8, Photo = "edinburgh.jpg"},
            new() {Id = 7, Name = "London", CountryId = 8, Photo = "london.jpg"},
            new() {Id = 8, Name = "Madrid", CountryId = 7, Photo = "madrid.jpg"},
            new() {Id = 9, Name = "New York", CountryId = 9, Photo = "new-york.jpg"},
            new() {Id = 10, Name = "Paris", CountryId = 3, Photo = "paris.jpg"},
            new() {Id = 11, Name = "Rome", CountryId = 6, Photo = "rome.jpg"},
            new() {Id = 12, Name = "Venice", CountryId = 6, Photo = "venice.jpg"}
        });
    }
}