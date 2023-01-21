using CityBreaks.Services.Interfaces;

namespace CityBreaks.Services;

public class UsPriceService : IPriceService
{
    public string GetLocation()
    {
        return "us";
    }

    public double CalculatePrice()
    {
        throw new NotImplementedException();
    }
}