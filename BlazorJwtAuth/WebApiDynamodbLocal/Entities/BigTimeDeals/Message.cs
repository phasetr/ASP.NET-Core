using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using WebApiDynamodbLocal.Constants;

namespace WebApiDynamodbLocal.Entities.BigTimeDeals;

[DynamoDBTable(AwsSettings.ConfigurationBigTimeDealsTable)]
public class Message : BaseEntity
{
    [DynamoDBProperty] public string Type { get; set; } = nameof(Message);
    [DynamoDBProperty] public string UserName { get; set; } = default!;
    [DynamoDBProperty] public string MessageId { get; set; } = default!;
    [DynamoDBProperty] public string Subject { get; set; } = default!;

    /// <summary>
    ///     Messageはクラス名と一致してエラーになるため変更。
    /// </summary>
    [DynamoDBProperty]
    public string Content { get; set; } = default!;

    [DynamoDBProperty] public string DealId { get; set; } = default!;
    [DynamoDBProperty] public DateTime SentAt { get; set; } = default!;
    [DynamoDBProperty] public bool UnRead { get; set; } = default!;

    public override string ToPk()
    {
        return $"MESSAGES#{UserName.ToLower()}";
    }

    public override string ToSk()
    {
        return $"MESSAGES#{MessageId}";
    }

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        var dynamodbItem = new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(Key.MessagePk(UserName))},
            {"SK", new AttributeValue(Key.MessageSk(MessageId))},
            {"Type", new AttributeValue(Type)},
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
