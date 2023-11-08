using System.Globalization;
using Amazon.DynamoDBv2.Model;
using KsuidDotNet;

namespace WebApiDynamodbLocal.Entities.ECommerce;

public class Order : BaseEntity
{
    public string UserName { get; set; } = default!;
    public string OrderId { get; set; } = default!;
    public Address Address { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = default!;
    public decimal TotalAmount { get; set; }
    public int NumberOfItems { get; set; }

    public static string GenerateOrderId(DateTime createdAt)
    {
        return Ksuid.NewKsuid(createdAt);
    }

    public static string OrderIdToGsi1Pk(string orderId)
    {
        return $"ORDER#{orderId}";
    }

    public static string UserNameToPk(string userName)
    {
        return $"CUSTOMER#{userName}";
    }

    public static string OrderIdToSk(string orderId)
    {
        return $"#ORDER#{orderId}";
    }

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
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
