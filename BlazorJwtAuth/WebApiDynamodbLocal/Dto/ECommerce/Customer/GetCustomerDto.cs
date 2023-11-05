using Common.Dto;

namespace WebApiDynamodbLocal.Dto.ECommerce.Customer;

public class GetCustomerDto : ResponseBaseDto
{
    /// <summary>
    ///     TODO: CustomerModelに切り替える
    /// </summary>
    public Entities.ECommerce.Customer? Customer { get; set; }
}
