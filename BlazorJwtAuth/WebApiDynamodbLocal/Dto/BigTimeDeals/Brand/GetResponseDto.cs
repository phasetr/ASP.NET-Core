using WebApiDynamodbLocal.Models.BigTimeDeals;

namespace WebApiDynamodbLocal.Dto.BigTimeDeals.Brand;

public class GetResponseDto : ResponseBaseDto
{
    public BrandModel? BrandModel { get; set; }
}
