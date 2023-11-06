using Common.Dto;
using WebApiDynamodbLocal.Dto.ECommerce.Order;

namespace WebApiDynamodbLocal.Services.ECommerce.Interfaces;

public interface IOrderService
{
    Task<PostResponseOrderDto> CreateAsync(PostOrderDto postOrderDto);
    Task<GetResponseOrderDto> GetByOrderIdAsync(string orderId);
    Task<GetResponseOrdersDto> GetByUserNameAsync(string userName);
    Task<ResponseBaseDto> PutStatusAsync(string userName, string orderId, string status);
}
