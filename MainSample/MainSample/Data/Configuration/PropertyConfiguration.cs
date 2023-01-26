using MainSample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MainSample.Data.Configuration;

public class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.HasData(new List<Property>
        {
            new()
            {
                Id = 1, CityId = 10, MaxNumberOfGuests = 1, DayRate = 81.00m, Name = "Hotel Paris",
                UserId = UserConfiguration.Guid1,
                Address = "Rue de Reynard"
            },
            new()
            {
                Id = 2, CityId = 4, MaxNumberOfGuests = 1, DayRate = 75.00m, Name = "Andersen Hotel",
                UserId = UserConfiguration.Guid1,
                Address = "Vester Volgade"
            },
            new()
            {
                Id = 3, CityId = 7, MaxNumberOfGuests = 2, DayRate = 72.00m, Name = "Ratz Hotel",
                UserId = UserConfiguration.Guid1, Address = "The Strand"
            }
        });
    }
}