﻿using CityBreaks.Services.Interfaces;

namespace CityBreaks.Services;

public class FrPriceService : IPriceService
{
    public double CalculatePrice()
    {
        throw new NotImplementedException();
    }

    public string GetLocation()
    {
        return "fr";
    }
}