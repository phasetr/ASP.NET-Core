using WebApiDynamodbLocal.Dto.BigTimeDeals.Deal;
using WebApiDynamodbLocal.Entities.BigTimeDeals;

namespace WebApiDynamodbLocal.Services.BigTimeDeals.Interfaces;

public interface IDealService
{
    Task<PostResponseDto> CreateAsync(Deal deal);
    Task<GetResponseDto> GetAsync(string dealId);

    Task<GetLatestDealsResponseDto> GetLatestDealsAsync(string brandName, DateOnly dateOnly, int limit = 25,
        int count = 0);
}
