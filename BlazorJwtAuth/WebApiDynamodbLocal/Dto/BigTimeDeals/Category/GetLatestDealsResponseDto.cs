using Common.Dto;
using WebApiDynamodbLocal.Models.BigTimeDeals;

namespace WebApiDynamodbLocal.Dto.BigTimeDeals.Category;

public class GetLatestDealsResponseDto : ResponseBaseDto
{
    public List<DealModel> LatestDeals { get; set; } = default!;
}
