using Client.Service.Services.Interfaces;

namespace Client.Service.Services;

public class ConsumptionTaxCalculator : IConsumptionTaxCalculator
{
    public int CalcTotalPrice(int price, DateTime date)
    {
        var rate = date >= new DateTime(2023, 1, 1) ? 1.1 : 1.08;
        return (int) (price * rate);
    }
}
