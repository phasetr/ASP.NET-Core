using Common.Dto;
using WebApiDynamodbLocal.Dto.ECommerce;

namespace WebApiDynamodbLocal.Services.ECommerce.Interfaces;

public interface IOrderService
{
    Task<ResponseBaseDto> CreateAsync(PostOrderDto postOrderDto);
}
