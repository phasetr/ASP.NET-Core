using Amazon.DynamoDBv2.Model;

namespace WebApiDynamodbLocal.Entities;

public abstract class BaseEntity
{
    public abstract Dictionary<string, AttributeValue> ToDynamoDbItem();
}
