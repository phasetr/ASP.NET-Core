using Common.Dto;
using WebApiDynamodbLocal.Models.BigTimeDeals;

namespace WebApiDynamodbLocal.Dto.BigTimeDeals.Category;

public class GetResponseDto : ResponseBaseDto
{
    public CategoryModel CategoryModel { get; set; } = default!;
}
