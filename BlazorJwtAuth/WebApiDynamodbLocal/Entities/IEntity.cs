using Amazon.DynamoDBv2.Model;

namespace WebApiDynamodbLocal.Entities;

public interface IEntity
{
    Dictionary<string, AttributeValue> ToDynamoDbItem();
}
