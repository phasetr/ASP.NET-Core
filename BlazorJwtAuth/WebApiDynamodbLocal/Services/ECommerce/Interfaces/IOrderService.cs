using Common.Dto;
using WebApiDynamodbLocal.Dto.ECommerce.Order;

namespace WebApiDynamodbLocal.Services.ECommerce.Interfaces;

public interface IOrderService
{
    Task<ResponseBaseDto> CreateAsync(PostOrderDto postOrderDto);
    Task<GetOrderDto> GetByOrderIdAsync(string orderId);
}
