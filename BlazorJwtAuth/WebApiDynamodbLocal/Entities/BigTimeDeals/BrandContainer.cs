using Amazon.DynamoDBv2.Model;

namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

public class BrandContainer : IEntity
{
    public List<string> Brands { get; set; } = default!;

    public Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(Key.BrandContainerPk())},
            {"SK", new AttributeValue(Key.BrandContainerSk())},
            {"Type", new AttributeValue(nameof(BrandContainer))},
            {"Brands", new AttributeValue {SS = Brands}}
        };
    }
}
