namespace WebApiDynamodbLocal.Models.BigTimeDeals;

public class DealModel
{
    public string DealId { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Link { get; set; } = default!;
    public decimal Price { get; set; }
    public string Category { get; set; } = default!;
    public string Brand { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}
