namespace CityBreaks.Services.Interfaces;

public interface IPriceService
{
    string GetLocation();
    double CalculatePrice();
}