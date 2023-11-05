using WebApiDynamodbLocal.Entities.ECommerce;

namespace WebApiDynamodbLocal.Dto.ECommerce.Customer;

public class PostCustomerDto
{
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Name { get; set; } = default!;
    public Dictionary<string, Address> Addresses { get; set; } = default!;
}
