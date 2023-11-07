using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using WebApiDynamodbLocal.Constants;

namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

[DynamoDBTable(AwsSettings.ConfigurationBigTimeDealsTable)]
public class User : BaseEntity
{
    [DynamoDBProperty] public string Type { get; set; } = nameof(User);
    [DynamoDBProperty] public string UserName { get; set; } = default!;
    [DynamoDBProperty] public string Name { get; set; } = default!;
    [DynamoDBProperty] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public override string ToPk()
    {
        return $"USER#{UserName.ToLower()}";
    }

    public override string ToSk()
    {
        return $"USER#{UserName.ToLower()}";
    }

    public string ToUserIndex()
    {
        return $"USER#{UserName.ToLower()}";
    }

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(ToPk())},
            {"SK", new AttributeValue(ToSk())},
            {"UserIndex", new AttributeValue(ToUserIndex())},
            {"Type", new AttributeValue(Type)},
            {"UserName", new AttributeValue(UserName)},
            {"Name", new AttributeValue(Name)},
            {"CreatedAt", new AttributeValue {S = CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}}
        };
    }
}
