using Amazon.DynamoDBv2.Model;

namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

public class Category : IEntity
{
    public string Name { get; set; } = default!;
    public string FeaturedDeals { get; set; } = default!;
    public int LikeCount { get; set; }
    public int WatchCount { get; set; }

    public Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(Key.CategoryPk(Name))},
            {"SK", new AttributeValue(Key.CategorySk(Name))},
            {"Type", new AttributeValue(nameof(Category))},
            {"Name", new AttributeValue(Name)},
            {"FeaturedDeals", new AttributeValue(FeaturedDeals)},
            {"LikeCount", new AttributeValue {N = LikeCount.ToString()}},
            {"WatchCount", new AttributeValue {N = WatchCount.ToString()}}
        };
    }
}
