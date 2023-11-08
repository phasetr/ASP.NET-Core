using System.ComponentModel.DataAnnotations;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

namespace WebApiDynamodbLocal.Entities.SessionStore;

public class Session : BaseEntity
{
    [Required]
    [DynamoDBProperty(AttributeName = "GSI1PK")]
    public string Gsi1Pk { get; set; } = default!;

    [Required]
    [DynamoDBProperty(AttributeName = "GSI1SK")]
    public string GsI1Sk { get; set; } = default!;

    [DynamoDBProperty] public string SessionId { get; set; } = default!;
    [DynamoDBProperty] public string UserName { get; set; } = default!;
    [DynamoDBProperty] public DateTime CreatedAt { get; set; }
    [DynamoDBProperty] public DateTime ExpiredAt { get; set; }
    [DynamoDBProperty] public int Ttl { get; set; }

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
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
