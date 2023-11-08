using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

namespace WebApiDynamodbLocal.Entities;

public abstract class BaseEntity
{
    [DynamoDBHashKey(AttributeName = "PK")]
    public string Pk { get; set; } = default!;

    [DynamoDBRangeKey(AttributeName = "SK")]
    public string Sk { get; set; } = default!;

    public abstract Dictionary<string, AttributeValue> ToDynamoDbItem();
}
