using System.ComponentModel.DataAnnotations;
using WebApiDynamodbLocal.Entities.ECommerce;
using WebApiDynamodbLocal.Models.ECommerce;

namespace WebApiDynamodbLocal.Dto.ECommerce.Order;

public class PostOrderDto
{
    [Required] public string UserName { get; set; } = string.Empty;
    [Required] public Address Address { get; set; } = default!;
    [Required] public string Status { get; set; } = string.Empty;
    [Required] public decimal TotalAmount { get; set; } = default!;
    [Required] public int NumberOfItems { get; set; } = default!;
    [Required] public OrderItemModel[] OrderItems { get; set; } = default!;
}
