using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using WebApiDynamodbLocal.Constants;

namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

[DynamoDBTable(AwsSettings.ConfigurationBigTimeDealsTable)]
public class Brand : BaseEntity
{
    [DynamoDBProperty] public string Name { get; set; } = default!;
    [DynamoDBProperty] public string LogoUrl { get; set; } = default!;
    [DynamoDBProperty] public int LikeCount { get; set; }
    [DynamoDBProperty] public int WatchCount { get; set; }

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(Key.BrandPk(Name))},
            {"SK", new AttributeValue(Key.BrandSk(Name))},
            {"Type", new AttributeValue(nameof(Brand))},
            {"Name", new AttributeValue(Name)},
            {"LogoUrl", new AttributeValue(LogoUrl)},
            {"LikeCount", new AttributeValue {N = LikeCount.ToString()}},
            {"WatchCount", new AttributeValue {N = WatchCount.ToString()}}
        };
    }
}
