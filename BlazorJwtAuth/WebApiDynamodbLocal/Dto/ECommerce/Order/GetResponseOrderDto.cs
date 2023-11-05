using Common.Dto;
using WebApiDynamodbLocal.Models.ECommerce;

namespace WebApiDynamodbLocal.Dto.ECommerce.Order;

public class GetResponseOrderDto : ResponseBaseDto
{
    public OrderModel? OrderModel { get; set; }
    public IEnumerable<OrderItemModel>? OrderItemModels { get; set; }
}
