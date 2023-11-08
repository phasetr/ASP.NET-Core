using WebApiDynamodbLocal.Models.ECommerce;

namespace WebApiDynamodbLocal.Dto.ECommerce.Order;

public class GetOrderResponseDto : ResponseBaseDto
{
    public OrderModel? OrderModel { get; set; }
    public IEnumerable<OrderItemModel>? OrderItemModels { get; set; }
}
