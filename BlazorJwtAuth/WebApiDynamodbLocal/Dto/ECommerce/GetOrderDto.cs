using Common.Dto;
using WebApiDynamodbLocal.Models.ECommerce;

namespace WebApiDynamodbLocal.Dto.ECommerce;

public class GetOrderDto : ResponseBaseDto
{
    public OrderModel? Order { get; set; }
    public IEnumerable<OrderItemModel>? OrderItems { get; set; }
}
