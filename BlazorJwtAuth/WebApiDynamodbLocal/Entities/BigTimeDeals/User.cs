using Amazon.DynamoDBv2.Model;

namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

public class User : BaseEntity
{
    public string UserName { get; set; } = default!;
    public string Name { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(Key.UserPk(UserName))},
            {"SK", new AttributeValue(Key.UserSk(UserName))},
            {"UserIndex", new AttributeValue(Key.UserUserIndex(UserName))},
            {"Type", new AttributeValue(nameof(User))},
            {"UserName", new AttributeValue(UserName)},
            {"Name", new AttributeValue(Name)},
            {"CreatedAt", new AttributeValue {S = CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}}
        };
    }
}
