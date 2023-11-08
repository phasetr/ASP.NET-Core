using Amazon.DynamoDBv2.Model;

namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

public class Message : IEntity
{
    public string UserName { get; set; } = default!;
    public string MessageId { get; set; } = default!;
    public string Subject { get; set; } = default!;

    /// <summary>
    ///     Messageはクラス名と一致してエラーになるため変更。
    /// </summary>
    public string Content { get; set; } = default!;

    public string DealId { get; set; } = default!;
    public DateTime SentAt { get; set; } = default!;
    public bool UnRead { get; set; } = default!;

    public Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        var dynamodbItem = new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(Key.MessagePk(UserName))},
            {"SK", new AttributeValue(Key.MessageSk(MessageId))},
            {"Type", new AttributeValue(nameof(Message))},
            {"UserName", new AttributeValue(UserName)},
            {"MessageId", new AttributeValue(MessageId)},
            {"Subject", new AttributeValue(Subject)},
            {"Content", new AttributeValue(Content)},
            {"DealId", new AttributeValue(DealId)},
            {"SentAt", new AttributeValue {S = SentAt.ToString("yyyy-MM-dd HH:mm:ss")}},
            {"UnRead", new AttributeValue {BOOL = UnRead}}
        };
        if (!UnRead) return dynamodbItem;
        dynamodbItem.Add("GSI1PK", new AttributeValue(Key.MessageGsi1Pk(UserName)));
        dynamodbItem.Add("GSI1SK", new AttributeValue(Key.MessageGsi1Sk(MessageId)));
        return dynamodbItem;
    }
}
