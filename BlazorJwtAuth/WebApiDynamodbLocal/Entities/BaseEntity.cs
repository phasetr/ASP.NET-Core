using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

namespace WebApiDynamodbLocal.Entities;

public abstract class BaseEntity
{
    public const string EntityName = "BaseEntity";

    [DynamoDBHashKey(AttributeName = "PK")]
    public string Pk { get; set; } = default!;

    [DynamoDBRangeKey(AttributeName = "SK")]
    public string Sk { get; set; } = default!;

    public abstract string ToPk(string key);
    public abstract string ToSk(string key);
    public abstract Dictionary<string, AttributeValue> ToDynamoDbItem();
}
