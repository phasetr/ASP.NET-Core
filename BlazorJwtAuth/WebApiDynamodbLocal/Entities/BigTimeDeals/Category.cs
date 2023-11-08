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

    public override string ToPk()
    {
        return $"CATEGORY#{Name.ToUpper()}";
    }

    public override string ToSk()
    {
        return $"CATEGORY#{Name.ToUpper()}";
    }

    public static string ToPk(string name)
    {
        return $"CATEGORY#{name.ToUpper()}";
    }

    public static string ToSk(string name)
    {
        return $"CATEGORY#{name.ToUpper()}";
    }

    public string ToGsi3Pk(DateTime dateTime)
    {
        return $"CATEGORY#{Name.ToUpper()}#{dateTime:yyyy-MM-dd}";
    }

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(ToPk())},
            {"SK", new AttributeValue(ToSk())},
            {"Type", new AttributeValue(Type)},
            {"Name", new AttributeValue(Name)},
            {"FeaturedDeals", new AttributeValue(FeaturedDeals)},
            {"LikeCount", new AttributeValue {N = LikeCount.ToString()}},
            {"WatchCount", new AttributeValue {N = WatchCount.ToString()}}
        };
    }
}
