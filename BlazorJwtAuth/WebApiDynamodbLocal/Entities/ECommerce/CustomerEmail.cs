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

    public override EntityKey Key()
    {
        return new EntityKey
        {
            Pk = $"{EntityName.ToUpper()}#{Email}",
            Sk = $"{EntityName.ToUpper()}#{Email}"
        };
    }

    public override CustomerEmail ToItem()
    {
        var key = Key();
        return new CustomerEmail
        {
            Pk = key.Pk,
            Sk = key.Sk,
            Type = Type,
            Email = Email,
            UserName = UserName
        };
    }

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        var key = Key();
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(key.Pk)},
            {"SK", new AttributeValue(key.Sk)},
            {"Type", new AttributeValue(Type)},
            {"Email", new AttributeValue(Email)},
            {"UserName", new AttributeValue(UserName)}
        };
    }
}
