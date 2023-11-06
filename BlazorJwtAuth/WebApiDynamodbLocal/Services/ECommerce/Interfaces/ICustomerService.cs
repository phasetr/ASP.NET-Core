using Common.Dto;
using WebApiDynamodbLocal.Dto.ECommerce.Customer;
using WebApiDynamodbLocal.Entities.ECommerce;

namespace WebApiDynamodbLocal.Services.ECommerce.Interfaces;

public interface ICustomerService
{
    Task<ResponseBaseDto> CreateAsync(Customer customer);
    Task<ResponseBaseDto> DeleteAddressAsync(string userName, string addressName);
    Task<GetResponseCustomerDto?> GetByUserNameAsync(string userName);
}
