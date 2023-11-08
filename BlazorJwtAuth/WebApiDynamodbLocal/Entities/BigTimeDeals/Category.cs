using Amazon.DynamoDBv2.Model;

namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

public class Category : IEntity
{
    public string Name { get; set; } = default!;
    public string FeaturedDeals { get; set; } = default!;
    public int LikeCount { get; set; }
    public int WatchCount { get; set; }

    public static string ToGsi3Pk(string name, DateTime dateTime)
    {
        return $"CATEGORY#{name.ToUpper()}#{dateTime:yyyy-MM-dd}";
    }

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
