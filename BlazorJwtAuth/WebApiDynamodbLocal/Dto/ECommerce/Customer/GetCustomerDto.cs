using Common.Dto;
using WebApiDynamodbLocal.Models.ECommerce;

namespace WebApiDynamodbLocal.Dto.ECommerce.Customer;

public class GetCustomerDto : ResponseBaseDto
{
    public CustomerModel? Customer { get; set; }
}
