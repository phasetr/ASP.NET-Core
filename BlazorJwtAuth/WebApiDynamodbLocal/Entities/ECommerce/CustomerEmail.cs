using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using WebApiDynamodbLocal.Constants;

namespace WebApiDynamodbLocal.Entities.ECommerce;

[DynamoDBTable(AwsSettings.ECommerceTable)]
public class CustomerEmail : BaseEntity
{
    public new const string EntityName = "CustomerEmail";
    [DynamoDBProperty] public string Type { get; set; } = default!;
    [DynamoDBProperty] public string Email { get; set; } = default!;
    [DynamoDBProperty] public string UserName { get; set; } = default!;

    public override string ToPk(string key)
    {
        return $"{nameof(CustomerEmail).ToUpper()}#{key}";
    }

    public override string ToSk(string key)
    {
        return $"{nameof(CustomerEmail).ToUpper()}#{key}";
    }

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(ToPk(Email))},
            {"SK", new AttributeValue(ToSk(Email))},
            {"Type", new AttributeValue(EntityName)},
            {"Email", new AttributeValue(Email)},
            {"UserName", new AttributeValue(UserName)}
        };
    }
}
