using CityBreaks.Models;
using CityBreaks.Pages;
using CityBreaks.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CityBreaks.Tests.Pages;

public class CityTests
{
    [Fact]
    public async Task Can_Use_Repository()
    {
        var countries = new List<Country>
        {
            new() {Id = 1, CountryName = "Country1", CountryCode = "c1", Cities = new List<City>()}
        };
        var cities = new List<City>
        {
            new() {Id = 1, Name = "C1", Photo = "P1", CountryId = 1, Country = countries[0], Properties = new List<Property>()},
            new() {Id = 2, Name = "C2", Photo = "P2", CountryId = 1, Country = countries[0], Properties = new List<Property>()}
        };
        var mockCities = new Mock<ICityRepository>();
        mockCities.Setup(m => m.Cities).Returns(cities.AsQueryable);
        var mockCountries = new Mock<ICountryRepository>();
        mockCountries.Setup(m => m.Countries).Returns(countries.AsQueryable);
        var mockCity = new Mock<ICityService>();
        mockCity.Setup(m => m.GetByNameAsync("C1").Result).Returns(cities[0]);
        var mockProperty = new Mock<IPropertyService>();
        var mockLogger = new Mock<ILogger<CityModel>>();

        var cityModel = new CityModel(mockCity.Object, mockProperty.Object, mockLogger.Object)
        {
            Name = "C1"
        };
        await cityModel.OnGetAsync();
        Assert.Equal(cities[0], cityModel.City);
    }
}