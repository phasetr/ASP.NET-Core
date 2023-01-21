using CityBreaks.Models;

namespace CityBreaks.Services.Interfaces;

public interface ICityRepository
{
    IQueryable<City> Cities { get; }
}