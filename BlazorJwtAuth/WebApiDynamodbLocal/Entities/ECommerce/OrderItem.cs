using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using WebApiDynamodbLocal.Constants;

namespace WebApiDynamodbLocal.Entities.ECommerce;

[DynamoDBTable(AwsSettings.ECommerceTable)]
public class OrderItem : BaseEntity
{
    [Required]
    [DynamoDBProperty(AttributeName = "GSI1PK")]
    public string Gsi1Pk { get; set; } = default!;

    [Required]
    [DynamoDBProperty(AttributeName = "GSI1SK")]
    public string GsI1Sk { get; set; } = default!;

    [DynamoDBProperty] public string Type { get; set; } = "OrderItem";
    [Required] [DynamoDBProperty] public string OrderId { get; set; } = default!;
    [Required] [DynamoDBProperty] public int ItemId { get; set; }
    [DynamoDBProperty] public string Description { get; set; } = default!;
    [DynamoDBProperty] public decimal Price { get; set; }
    [DynamoDBProperty] public int Amount { get; set; }

    public string ToGsi1Pk()
    {
        return $"ORDER#{OrderId}";
    }

    public string ToGsi1Sk()
    {
        return $"ITEM#{ItemId}";
    }

    public override string ToPk()
    {
        return $"ORDER#{OrderId}#ITEM#{ItemId}";
    }

    public override string ToSk()
    {
        return $"ORDER#{OrderId}#ITEM#{ItemId}";
    }

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(ToPk())},
            {"SK", new AttributeValue(ToSk())},
            {"GSI1PK", new AttributeValue(ToGsi1Pk())},
            {"GSI1SK", new AttributeValue(ToGsi1Sk())},
            {"Type", new AttributeValue(nameof(OrderItem))},
            {"OrderId", new AttributeValue(OrderId)},
            {"ItemId", new AttributeValue {N = ItemId.ToString()}},
            {"Description", new AttributeValue(Description)},
            {"Price", new AttributeValue {N = Price.ToString(CultureInfo.InvariantCulture)}},
            {"Amount", new AttributeValue {N = Amount.ToString()}}
        };
    }
}
