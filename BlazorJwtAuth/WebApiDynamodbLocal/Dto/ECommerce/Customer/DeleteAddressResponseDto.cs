using Common.Dto;

namespace WebApiDynamodbLocal.Dto.ECommerce.Customer;

public class DeleteAddressResponseDto : ResponseBaseDto
{
    public string UserName { get; set; } = default!;
    public string AddressName { get; set; } = default!;
}
