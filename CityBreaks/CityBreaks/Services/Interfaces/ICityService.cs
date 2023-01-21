using CityBreaks.Models;

namespace CityBreaks.Services.Interfaces;

public interface ICityService
{
    Task<List<City>> GetAllAsync();
    Task<City> GetByNameAsync(string name);
    Task<City> GetByIdAsync(int id);
    Task<City> CreateAsync(City city);
    Task<City> UpdateAsync(City city);
}