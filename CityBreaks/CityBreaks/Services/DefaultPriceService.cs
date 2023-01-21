using CityBreaks.Services.Interfaces;

namespace CityBreaks.Services;

public class DefaultPriceService : IPriceService
{
    public double CalculatePrice()
    {
        throw new NotImplementedException();
    }

    public string GetLocation()
    {
        return "XX";
    }
}