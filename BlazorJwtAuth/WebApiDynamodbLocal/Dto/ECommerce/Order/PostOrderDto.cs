using System.ComponentModel.DataAnnotations;
using WebApiDynamodbLocal.Entities.ECommerce;
using WebApiDynamodbLocal.Models.ECommerce;

namespace WebApiDynamodbLocal.Dto.ECommerce.Order;

public class PostOrderDto
{
    [Required] public string UserName { get; set; } = string.Empty;
    [Required] public Address Address { get; set; } = default!;
    [Required] public decimal TotalAmount { get; set; }
    [Required] public int NumberOfItems { get; set; }
    [Required] public OrderItemModel[] OrderItemModels { get; set; } = default!;
}
