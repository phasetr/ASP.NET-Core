using Amazon.DynamoDBv2.Model;

namespace WebApiDynamodbLocal.Entities.SessionStore;

public class Session : IEntity
{
    public string SessionId { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiredAt { get; set; }
    public int Ttl { get; set; }

    public Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(Key.SessionPk(SessionId))},
            {"SK", new AttributeValue(Key.SessionSk(SessionId))},
            {"GSI1PK", new AttributeValue(Key.SessionGsi1Pk(UserName))},
            {"GSI1SK", new AttributeValue(Key.SessionGsi1Sk(UserName))},
            {"Type", new AttributeValue(nameof(Session))},
            {"SessionId", new AttributeValue(SessionId)},
            {"UserName", new AttributeValue(UserName)},
            {"CreatedAt", new AttributeValue(CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))},
            {"ExpiredAt", new AttributeValue(ExpiredAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))},
            {"Ttl", new AttributeValue(Ttl.ToString())}
        };
    }
}
