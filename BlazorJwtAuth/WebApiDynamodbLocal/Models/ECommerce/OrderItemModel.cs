using System.ComponentModel.DataAnnotations;

namespace WebApiDynamodbLocal.Models.ECommerce;

public class OrderItemModel
{
    [Required] public string OrderItemId { get; set; } = string.Empty;
    [Required] public string Description { get; set; } = string.Empty;
    [Required] public decimal Price { get; set; }
    [Required] public int Amount { get; set; }
}
