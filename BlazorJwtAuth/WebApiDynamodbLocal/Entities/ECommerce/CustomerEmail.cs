using Amazon.DynamoDBv2.Model;

namespace WebApiDynamodbLocal.Entities.ECommerce;

public class CustomerEmail : BaseEntity
{
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(Key.CustomerEmailPk(Email))},
            {"SK", new AttributeValue(Key.CustomerEmailSk(Email))},
            {"Type", new AttributeValue(nameof(CustomerEmail))},
            {"Email", new AttributeValue(Email)},
            {"UserName", new AttributeValue(UserName)}
        };
    }
}
