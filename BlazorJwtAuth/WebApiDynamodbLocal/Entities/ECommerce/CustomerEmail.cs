using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using WebApiDynamodbLocal.Constants;

namespace WebApiDynamodbLocal.Entities.ECommerce;

[DynamoDBTable(AwsSettings.ECommerceTable)]
public class CustomerEmail : BaseEntity
{
    [DynamoDBProperty] public string Type { get; set; } = nameof(CustomerEmail);
    [DynamoDBProperty] public string Email { get; set; } = default!;
    [DynamoDBProperty] public string UserName { get; set; } = default!;

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(Key.CustomerEmailPk(Email))},
            {"SK", new AttributeValue(Key.CustomerEmailSk(Email))},
            {"Type", new AttributeValue(Type)},
            {"Email", new AttributeValue(Email)},
            {"UserName", new AttributeValue(UserName)}
        };
    }
}
