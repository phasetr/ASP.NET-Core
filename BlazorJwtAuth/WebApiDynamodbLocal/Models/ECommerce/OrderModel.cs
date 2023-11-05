using System.ComponentModel.DataAnnotations;
using WebApiDynamodbLocal.Entities.ECommerce;

namespace WebApiDynamodbLocal.Models.ECommerce;

public class OrderModel
{
    [Required] public string UserName { get; set; } = string.Empty;
    [Required] public string OrderId { get; set; } = string.Empty;
    [Required] public Address Address { get; set; } = default!;
    [Required] public string CreatedAt { get; set; } = default!;
    [Required] public string Status { get; set; } = string.Empty;
    [Required] public string TotalAmount { get; set; } = string.Empty;
    [Required] public string NumberOfItems { get; set; } = string.Empty;
}
