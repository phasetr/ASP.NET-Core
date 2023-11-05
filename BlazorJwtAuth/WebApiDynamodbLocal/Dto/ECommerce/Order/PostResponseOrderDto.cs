using Common.Dto;

namespace WebApiDynamodbLocal.Dto.ECommerce.Order;

public class PostResponseOrderDto : ResponseBaseDto
{
    public string? OrderId { get; set; }
}
