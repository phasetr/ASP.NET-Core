using System.Globalization;
using Amazon.DynamoDBv2.Model;

namespace WebApiDynamodbLocal.Entities.ECommerce;

public class Order : IEntity
{
    public string UserName { get; set; } = default!;
    public string OrderId { get; set; } = default!;
    public Address Address { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = default!;
    public decimal TotalAmount { get; set; }
    public int NumberOfItems { get; set; }

    public Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(Key.OrderPk(UserName))},
            {"SK", new AttributeValue(Key.OrderSk(OrderId))},
            {"GSI1PK", new AttributeValue(Key.OrderGsi1Pk(OrderId))},
            {"GSI1SK", new AttributeValue(Key.OrderGsi1Sk(OrderId))},
            {"Type", new AttributeValue(nameof(Order))},
            {"UserName", new AttributeValue(UserName)},
            {"OrderId", new AttributeValue(OrderId)},
            {"Address", new AttributeValue {M = Address.ToDynamoDbItem()}},
            {"CreatedAt", new AttributeValue {S = CreatedAt.ToString(CultureInfo.InvariantCulture)}},
            {"Status", new AttributeValue(Status)},
            {"TotalAmount", new AttributeValue {N = TotalAmount.ToString(CultureInfo.InvariantCulture)}},
            {"NumberOfItems", new AttributeValue {N = NumberOfItems.ToString()}}
        };
    }
}
