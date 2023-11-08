using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using WebApiDynamodbLocal.Constants;

namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

[DynamoDBTable(AwsSettings.ConfigurationBigTimeDealsTable)]
public class BrandContainer : BaseEntity
{
    [DynamoDBProperty] public string Type { get; set; } = nameof(BrandContainer);
    [DynamoDBProperty] public List<string> Brands { get; set; } = default!;

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(Key.BrandContainerPk())},
            {"SK", new AttributeValue(Key.BrandContainerSk())},
            {"Type", new AttributeValue(Type)},
            {"Brands", new AttributeValue {SS = Brands}}
        };
    }
}
