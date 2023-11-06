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

    [DynamoDBProperty] public string Type { get; set; } = "Session";
    [DynamoDBProperty] public string SessionId { get; set; } = default!;
    [DynamoDBProperty] public string UserName { get; set; } = default!;
    [DynamoDBProperty] public DateTime CreatedAt { get; set; } = default!;
    [DynamoDBProperty] public DateTime ExpiredAt { get; set; } = default!;
    [DynamoDBProperty] public int Ttl { get; set; } = default!;

    public override string ToPk()
    {
        return $"{nameof(Session).ToUpper()}#{SessionId}";
    }

    public override string ToSk()
    {
        return $"{nameof(Session).ToUpper()}#{SessionId}";
    }

    public static string UserNameToPk(string userName)
    {
        return $"{nameof(Session).ToUpper()}#{userName}";
    }

    public static string UserNameToSk(string userName)
    {
        return $"{nameof(Session).ToUpper()}#{userName}";
    }

    public static string SessionIdToPk(string sessionId)
    {
        return $"{nameof(Session).ToUpper()}#{sessionId}";
    }

    public static string SessionIdToSk(string sessionId)
    {
        return $"{nameof(Session).ToUpper()}#{sessionId}";
    }

    public static string UserNameToGsi1Pk(string userName)
    {
        return $"{nameof(Session).ToUpper()}#{userName}";
    }

    public static string UserNameToGsi1Sk(string userName)
    {
        return $"{nameof(Session).ToUpper()}#{userName}";
    }

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(ToPk())},
            {"SK", new AttributeValue(ToSk())},
            {"GSI1PK", new AttributeValue(UserNameToGsi1Pk(UserName))},
            {"GSI1SK", new AttributeValue(UserNameToGsi1Sk(UserName))},
            {"Type", new AttributeValue(nameof(Session))},
            {"SessionId", new AttributeValue(SessionId)},
            {"UserName", new AttributeValue(UserName)},
            {"CreatedAt", new AttributeValue(CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))},
            {"ExpiredAt", new AttributeValue(ExpiredAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))},
            {"Ttl", new AttributeValue(Ttl.ToString())}
        };
    }
}
