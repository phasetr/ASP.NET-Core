using System.Globalization;
using Amazon.DynamoDBv2.Model;

namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

public class Deal : IEntity
{
    public string DealId { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Link { get; set; } = default!;
    public decimal Price { get; set; }
    public string Category { get; set; } = default!;
    public string Brand { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    public Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(Key.DealPk(DealId))},
            {"SK", new AttributeValue(Key.DealSk(DealId))},
            {"GSI1PK", new AttributeValue(Key.DealGsi1Pk(CreatedAt))},
            {"GSI1SK", new AttributeValue(Key.DealGsi1Sk(DealId))},
            {"GSI2PK", new AttributeValue(Key.DealGsi2Pk(Brand, CreatedAt))},
            {"GSI2SK", new AttributeValue(Key.DealGsi2Sk(DealId))},
            {"GSI3PK", new AttributeValue(Key.DealGsi3Pk(Category, CreatedAt))},
            {"GSI3SK", new AttributeValue(Key.DealGsi3Sk(DealId))},
            {"Type", new AttributeValue(nameof(Deal))},
            {"DealId", new AttributeValue(DealId)},
            {"Title", new AttributeValue(Title)},
            {"Link", new AttributeValue(Link)},
            {"Price", new AttributeValue {N = Price.ToString(CultureInfo.InvariantCulture)}},
            {"Category", new AttributeValue(Category)},
            {"Brand", new AttributeValue(Brand)},
            {"CreatedAt", new AttributeValue(CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"))}
        };
    }
}
