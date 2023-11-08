using Common.Dto;
using WebApiDynamodbLocal.Dto.ECommerce.Order;
using ResponseBaseDto = WebApiDynamodbLocal.Dto.ResponseBaseDto;

namespace WebApiDynamodbLocal.Services.ECommerce.Interfaces;

public interface IOrderService
{
    Task<ResponseBaseWithKeyDto> CreateAsync(PostOrderDto postOrderDto);
    Task<GetOrderResponseDto> GetByOrderIdAsync(string orderId);
    Task<GetOrdersResponseDto> GetByUserNameAsync(string userName);
    Task<ResponseBaseDto> PutStatusAsync(string userName, string orderId, string status);
}
