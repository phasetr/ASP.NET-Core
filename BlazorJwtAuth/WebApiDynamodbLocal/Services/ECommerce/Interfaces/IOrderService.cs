using Common.Dto;
using WebApiDynamodbLocal.Dto.ECommerce.Order;

namespace WebApiDynamodbLocal.Services.ECommerce.Interfaces;

public interface IOrderService
{
    Task<ResponseBaseDto> CreateAsync(PostOrderDto postOrderDto);
    Task<GetOrderResponseDto> GetByOrderIdAsync(string orderId);
    Task<GetOrdersResponseDto> GetByUserNameAsync(string userName);
    Task<ResponseBaseDto> PutStatusAsync(string userName, string orderId, string status);
}
