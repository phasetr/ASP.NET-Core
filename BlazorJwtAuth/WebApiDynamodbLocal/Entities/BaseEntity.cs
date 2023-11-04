using System.Runtime.CompilerServices;
using Amazon.DynamoDBv2.DataModel;

namespace WebApiDynamodbLocal.Entities;

public abstract class BaseEntity
{
    public const string EntityName = "BaseEntity";

    [DynamoDBHashKey(AttributeName = "PK")]
    public string Pk { get; set; } = default!;

    [DynamoDBRangeKey(AttributeName = "SK")]
    public string Sk { get; set; } = default!;

    public abstract EntityKey Key();
    public abstract BaseEntity ToItem();
    public abstract Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue> ToDynamoDbItem();
}
