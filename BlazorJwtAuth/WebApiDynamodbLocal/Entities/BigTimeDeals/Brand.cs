using Amazon.DynamoDBv2.Model;

namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

public class Brand : IEntity
{
    public string Name { get; set; } = default!;
    public string LogoUrl { get; set; } = default!;
    public int LikeCount { get; set; }
    public int WatchCount { get; set; }

    public Dictionary<string, AttributeValue> ToDynamoDbItem()
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
