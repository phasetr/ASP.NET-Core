using Common.Dto;
using WebApiDynamodbLocal.Models.BigTimeDeals;

namespace WebApiDynamodbLocal.Dto.BigTimeDeals.Deal;

public class GetResponseDto : ResponseBaseDto
{
    public DealModel? DealModel { get; set; }
}
