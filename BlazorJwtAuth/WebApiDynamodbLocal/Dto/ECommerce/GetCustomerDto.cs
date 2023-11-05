using Common.Dto;
using WebApiDynamodbLocal.Entities.ECommerce;

namespace WebApiDynamodbLocal.Dto.ECommerce;

public class GetCustomerDto : ResponseBaseDto
{
    /// <summary>
    ///     TODO: CustomerModelに切り替える
    /// </summary>
    public Customer? Customer { get; set; }
}
