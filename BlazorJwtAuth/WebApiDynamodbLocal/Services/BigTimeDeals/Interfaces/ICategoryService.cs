using WebApiDynamodbLocal.Dto;
using WebApiDynamodbLocal.Dto.BigTimeDeals.Category;
using WebApiDynamodbLocal.Entities.BigTimeDeals;

namespace WebApiDynamodbLocal.Services.BigTimeDeals.Interfaces;

public interface ICategoryService
{
    Task<GetResponseDto> GetAsync(string name);
    Task<GetCategoryAndLatestDealsResponseDto> GetCategoryAndLatestDealsAsync(string name);
    Task<GetLatestDealsResponseDto> GetLatestDealsAsync(string name, DateTime createdAt, int limit = 25, int count = 0);

    /// <summary>
    ///     updateCategory用のサービスメソッド。
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    Task<ResponseBaseDto> CreateAsync(Category category);
}
