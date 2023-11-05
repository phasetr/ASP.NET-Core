using Common.Dto;
using WebApiDynamodbLocal.Dto.ECommerce.Customer;
using WebApiDynamodbLocal.Entities.ECommerce;

namespace WebApiDynamodbLocal.Services.ECommerce.Interfaces;

public interface ICustomerService
{
    Task<ResponseBaseDto> CreateAsync(Customer customer);
    Task<bool> DeleteAsync(Customer customer);
    Task<IList<Customer>> GetCustomersAsync(int limit = 10);
    Task<GetResponseCustomerDto?> GetByUserNameAsync(string userName);
    Task<bool> UpdateAsync(Customer customer);
}
