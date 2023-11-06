using WebApiDynamodbLocal.Entities.ECommerce;

namespace WebApiDynamodbLocal.Dto.ECommerce.Customer;

public class PutAddressDto
{
    public string UserName { get; set; } = default!;
    public string AddressName { get; set; } = default!;
    public Address Address { get; set; } = default!;
}
