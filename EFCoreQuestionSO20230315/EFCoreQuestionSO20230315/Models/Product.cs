namespace EFCoreQuestionSO20230315.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int ShopId { get; set; }
    public Shop Shop { get; set; } = default!;
    public bool IsDeleted { get; set; } = false;
}
