using Common.Dto;
using WebApiDynamodbLocal.Models.BigTimeDeals;

namespace WebApiDynamodbLocal.Dto.BigTimeDeals.Category;

public class GetCategoryAndLatestDealsResponseDto : ResponseBaseDto
{
    public CategoryModel? CategoryModel { get; set; }
    public List<DealModel>? LatestDealModels { get; set; }
}
