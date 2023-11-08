using Common.Dto;
using WebApiDynamodbLocal.Dto.BigTimeDeals.Category;
using WebApiDynamodbLocal.Entities.BigTimeDeals;

namespace WebApiDynamodbLocal.Services.BigTimeDeals.Interfaces;

public interface ICategoryService
{
    Task<GetResponseDto> GetAsync(string name);

    /// <summary>
    ///     updateCategory用のサービスメソッド。
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    Task<ResponseBaseDto> CreateAsync(Category category);
}
