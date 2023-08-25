using BlazorJwtAuth.Client.Service.Services.Interfaces;

namespace BlazorJwtAuth.Client.Service.Services;

public class ConsumptionTaxCalculator: IConsumptionTaxCalculator {
    public int CalcTotalPrice(int price) {
        var rate = DateTime.Today >= new DateTime(2019, 10, 1) ? 1.1 : 1.08;
        return (int)(price * rate);
    }
}

