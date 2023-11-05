using WebApiDynamodbLocal.Dto.ECommerce.Order;

namespace WebApiDynamodbLocal.Services.ECommerce.Interfaces;

public interface IOrderService
{
    Task<PostResponseOrderDto> CreateAsync(PostOrderDto postOrderDto, DateTime dateTime);
    Task<GetResponseOrderDto> GetByOrderIdAsync(string orderId);
}
