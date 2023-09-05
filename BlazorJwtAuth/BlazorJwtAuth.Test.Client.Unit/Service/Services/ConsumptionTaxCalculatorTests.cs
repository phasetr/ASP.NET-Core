using System;
using BlazorJwtAuth.Client.Service.Services;

namespace BlazorJwtAuth.Test.Client.Unit.Service.Services;

public class ConsumptionTaxCalculatorTests
{
    [Fact]
    public void CalcTotalPrice_2022_12_31()
    {
        var target = new ConsumptionTaxCalculator();
        var actual = target.CalcTotalPrice(100, new DateTime(2022, 12, 31));
        Assert.Equal(108, actual);
    }

    [Fact]
    public void CalcTotalPrice_2023_01_01()
    {
        var target = new ConsumptionTaxCalculator();
        var actual = target.CalcTotalPrice(100, new DateTime(2023, 01, 01));
        Assert.Equal(110, actual);
    }
}
