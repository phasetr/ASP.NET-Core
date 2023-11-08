using Common.Dto;
using WebApiDynamodbLocal.Models.BigTimeDeals;

namespace WebApiDynamodbLocal.Dto.BigTimeDeals.Deal;

public class GetLatestDealsResponseDto : ResponseBaseDto
{
    public List<DealModel> DealModels { get; set; } = default!;
}
