using Common.Dto;
using WebApiDynamodbLocal.Dto;
using WebApiDynamodbLocal.Entities.ECommerce;

namespace WebApiDynamodbLocal.Services.Interfaces;

public interface ICustomerService
{
    Task<ResponseBaseDto> CreateAsync(Customer customer);
    Task<bool> DeleteAsync(Customer customer);
    Task<IList<Customer>> GetCustomersAsync(int limit = 10);
    Task<GetCustomerDto?> GetByUserNameAsync(string userName);
    Task<bool> UpdateAsync(Customer customer);
}
