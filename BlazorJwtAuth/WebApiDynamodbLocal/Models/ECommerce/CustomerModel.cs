using WebApiDynamodbLocal.Entities.ECommerce;

namespace WebApiDynamodbLocal.Models.ECommerce;

public class CustomerModel
{
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Name { get; set; } = default!;
    public Dictionary<string, Address> Addresses { get; set; } = default!;
}
