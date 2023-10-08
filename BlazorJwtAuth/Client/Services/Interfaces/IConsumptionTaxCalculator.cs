namespace Client.Services.Interfaces;

public interface IConsumptionTaxCalculator
{
    int CalcTotalPrice(int price, DateTime date);
}
