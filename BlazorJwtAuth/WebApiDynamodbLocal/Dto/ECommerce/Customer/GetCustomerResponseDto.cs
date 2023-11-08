using Common.Dto;
using WebApiDynamodbLocal.Models.ECommerce;

namespace WebApiDynamodbLocal.Dto.ECommerce.Customer;

public class GetCustomerResponseDto : ResponseBaseDto
{
    public CustomerModel? CustomerModel { get; set; }
}
