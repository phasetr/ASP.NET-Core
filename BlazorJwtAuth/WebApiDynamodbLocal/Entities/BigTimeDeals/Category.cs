using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using WebApiDynamodbLocal.Constants;

namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

[DynamoDBTable(AwsSettings.ConfigurationBigTimeDealsTable)]
public class Category : BaseEntity
{
    [DynamoDBProperty] public string Type { get; set; } = nameof(Category);
    [DynamoDBProperty] public string Name { get; set; } = default!;
    [DynamoDBProperty] public string FeaturedDeals { get; set; } = default!;
    [DynamoDBProperty] public int LikeCount { get; set; }
    [DynamoDBProperty] public int WatchCount { get; set; }

    public static string ToPk(string name)
    {
        return $"CATEGORY#{name.ToUpper()}";
    }

    public static string ToSk(string name)
    {
        return $"CATEGORY#{name.ToUpper()}";
    }

    public static string ToGsi3Pk(string name, DateTime dateTime)
    {
        return $"CATEGORY#{name.ToUpper()}#{dateTime:yyyy-MM-dd}";
    }

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(Key.CategoryPk(Name))},
            {"SK", new AttributeValue(Key.CategorySk(Name))},
            {"Type", new AttributeValue(Type)},
            {"Name", new AttributeValue(Name)},
            {"FeaturedDeals", new AttributeValue(FeaturedDeals)},
            {"LikeCount", new AttributeValue {N = LikeCount.ToString()}},
            {"WatchCount", new AttributeValue {N = WatchCount.ToString()}}
        };
    }
}
