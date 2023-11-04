using Common.Dto;
using WebApiDynamodbLocal.Entities.ECommerce;

namespace WebApiDynamodbLocal.Dto;

public class GetCustomerDto : ResponseBaseDto
{
    public Customer? Customer { get; set; }
}
