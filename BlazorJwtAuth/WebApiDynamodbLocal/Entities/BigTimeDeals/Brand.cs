using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using WebApiDynamodbLocal.Constants;

namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

[DynamoDBTable(AwsSettings.ConfigurationBigTimeDealsTable)]
public class Brand : BaseEntity
{
    [DynamoDBProperty] public string Type { get; set; } = nameof(Brand);
    [DynamoDBProperty] public string Name { get; set; } = default!;
    [DynamoDBProperty] public string LogoUrl { get; set; } = default!;
    [DynamoDBProperty] public int LikeCount { get; set; } = default!;
    [DynamoDBProperty] public int WatchCount { get; set; } = default!;

    public override string ToPk()
    {
        return $"BRAND#{Name.ToUpper()}";
    }

    public override string ToSk()
    {
        return $"BRAND#{Name.ToUpper()}";
    }

    public static string NameToPk(string name)
    {
        return $"BRAND#{name.ToUpper()}";
    }

    public static string NameToSk(string name)
    {
        return $"BRAND#{name.ToUpper()}";
    }

    public string ToGsi2Pk(DateTime dateTime)
    {
        return $"BRAND#{Name.ToUpper()}#{dateTime:yyyy-MM-dd HH:mm:ss}";
    }

    public string ToGsi2Sk(DateTime dateTime)
    {
        return $"BRAND#{Name.ToUpper()}#{dateTime:yyyy-MM-dd HH:mm:ss}";
    }

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(ToPk())},
            {"SK", new AttributeValue(ToSk())},
            {"Type", new AttributeValue(Type)},
            {"Name", new AttributeValue(Name)},
            {"LogoUrl", new AttributeValue(LogoUrl)},
            {"LikeCount", new AttributeValue {N = LikeCount.ToString()}},
            {"WatchCount", new AttributeValue {N = WatchCount.ToString()}}
        };
    }
}
