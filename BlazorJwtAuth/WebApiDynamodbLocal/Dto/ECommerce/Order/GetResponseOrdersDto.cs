using Common.Dto;
using WebApiDynamodbLocal.Models.ECommerce;

namespace WebApiDynamodbLocal.Dto.ECommerce.Order;

public class GetResponseOrdersDto : ResponseBaseDto
{
    public string? UserName { get; set; }
    public List<OrderModel> OrderModels { get; set; } = default!;
}
