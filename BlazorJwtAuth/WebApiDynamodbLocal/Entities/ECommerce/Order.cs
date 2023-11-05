using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using KsuidDotNet;
using WebApiDynamodbLocal.Constants;

namespace WebApiDynamodbLocal.Entities.ECommerce;

[DynamoDBTable(AwsSettings.ECommerceTable)]
public class Order : BaseEntity
{
    [Required]
    [DynamoDBProperty(AttributeName = "GSI1PK")]
    public string Gsi1Pk { get; set; } = default!;

    [Required]
    [DynamoDBProperty(AttributeName = "GSI1SK")]
    public string GsI1Sk { get; set; } = default!;

    [DynamoDBProperty] public string Type { get; set; } = "Order";
    [DynamoDBProperty] public string UserName { get; set; } = default!;
    [DynamoDBProperty] public string OrderId { get; set; } = default!;
    [DynamoDBProperty] public Address Address { get; set; } = default!;
    [DynamoDBProperty] public DateTime CreatedAt { get; set; } = default!;
    [DynamoDBProperty] public string Status { get; set; } = default!;
    [DynamoDBProperty] public decimal TotalAmount { get; set; } = default!;
    [DynamoDBProperty] public int NumberOfItems { get; set; } = default!;

    public static string GenerateOrderId(DateTime createdAt)
    {
        return Ksuid.NewKsuid(createdAt);
    }

    public static string OrderIdToGsi1Pk(string orderId)
    {
        return $"ORDER#{orderId}";
    }

    public override string ToPk()
    {
        return $"CUSTOMER#{UserName}";
    }

    public override string ToSk()
    {
        return $"#ORDER#{OrderId}";
    }

    public string ToGsi1Pk()
    {
        return $"ORDER#{OrderId}";
    }

    public string ToGsi1Sk()
    {
        return $"ORDER#{OrderId}";
    }

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(ToPk())},
            {"SK", new AttributeValue(ToSk())},
            {"GSI1PK", new AttributeValue(ToGsi1Pk())},
            {"GSI1SK", new AttributeValue(ToGsi1Sk())},
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
