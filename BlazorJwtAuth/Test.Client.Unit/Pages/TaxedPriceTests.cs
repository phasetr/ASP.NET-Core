using System;
using Client.Pages;
using Client.Service.Services;
using Client.Service.Services.Interfaces;

namespace Test.Client.Unit.Pages;

public class TaxedPriceTests : TestContext
{
    [Fact]
    public void H2_Test()
    {
        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConsumptionTaxCalculator, ConsumptionTaxCalculator>();
        var cut = ctx.RenderComponent<TaxedPrice>();
        cut.Find("h2").MarkupMatches("<h2>Taxed Price</h2>");
    }

    [Fact]
    public void PriceWithTax_Rate8percent_Test()
    {
        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConsumptionTaxCalculator, ConsumptionTaxCalculator>();
        var cut = ctx.RenderComponent<TaxedPrice>(p =>
            p.Add(d => d.Date, new DateTime(2022, 9, 30))
                .Add(d => d.Price, 1000));
        var v0 = cut.Find("#priceWithTax").InnerHtml;
        Assert.Equal("Price with tax: ¥0", v0);
        cut.Find("input[type=button]").Click();
        var v = cut.Find("#priceWithTax").InnerHtml;
        Assert.Equal("Price with tax: ¥1080", v);
    }

    [Fact]
    public void PriceWithTax_Rate10percent_Test()
    {
        using var ctx = new TestContext();
        ctx.Services.AddSingleton<IConsumptionTaxCalculator, ConsumptionTaxCalculator>();
        var cut = ctx.RenderComponent<TaxedPrice>(p =>
            p.Add(d => d.Date, new DateTime(2023, 9, 30))
                .Add(d => d.Price, 1000));
        var v0 = cut.Find("#priceWithTax").InnerHtml;
        Assert.Equal("Price with tax: ¥0", v0);
        cut.Find("input[type=button]").Click();
        var v = cut.Find("#priceWithTax").InnerHtml;
        Assert.Equal("Price with tax: ¥1100", v);
    }
}
