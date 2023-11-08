using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Amazon.DynamoDBv2.Model;

namespace WebApiDynamodbLocal.Entities.ECommerce;

public class OrderItem : BaseEntity
{
    [Required] public string OrderId { get; set; } = default!;
    [Required] public int ItemId { get; set; }
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public int Amount { get; set; }

    public override Dictionary<string, AttributeValue> ToDynamoDbItem()
    {
        return new Dictionary<string, AttributeValue>
        {
            {"PK", new AttributeValue(Key.OrderItemPk(OrderId, ItemId.ToString()))},
            {"SK", new AttributeValue(Key.OrderItemSk(OrderId, ItemId.ToString()))},
            {"GSI1PK", new AttributeValue(Key.OrderItemGsi1Pk(OrderId))},
            {"GSI1SK", new AttributeValue(Key.OrderItemGsi1Sk(ItemId.ToString()))},
            {"Type", new AttributeValue(nameof(OrderItem))},
            {"OrderId", new AttributeValue(OrderId)},
            {"ItemId", new AttributeValue {N = ItemId.ToString()}},
            {"Description", new AttributeValue(Description)},
            {"Price", new AttributeValue {N = Price.ToString(CultureInfo.InvariantCulture)}},
            {"Amount", new AttributeValue {N = Amount.ToString()}}
        };
    }
}
