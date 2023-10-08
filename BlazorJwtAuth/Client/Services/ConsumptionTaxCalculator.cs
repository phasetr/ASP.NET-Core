using Client.Services.Interfaces;

namespace Client.Services;

public class ConsumptionTaxCalculator : IConsumptionTaxCalculator
{
    public int CalcTotalPrice(int price, DateTime date)
    {
        var rate = date >= new DateTime(2023, 1, 1) ? 1.1 : 1.08;
        return (int) (price * rate);
    }
}
