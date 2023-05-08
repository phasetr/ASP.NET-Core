namespace EFCoreQuestionSO20230315.Models;

public class Shop
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public IList<PaymentMethod> PaymentMethods { get; set; } = default!;

    public ICollection<ApplicationUserShop> ApplicationUserShops { get; set; } = default!;
}
