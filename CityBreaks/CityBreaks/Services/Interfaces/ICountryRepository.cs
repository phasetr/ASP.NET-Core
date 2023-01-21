using CityBreaks.Models;

namespace CityBreaks.Services.Interfaces;

public interface ICountryRepository
{
    IQueryable<Country> Countries { get; }
}