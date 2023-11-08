using WebApiDynamodbLocal.Dto;
using WebApiDynamodbLocal.Dto.BigTimeDeals.Brand;
using WebApiDynamodbLocal.Entities.BigTimeDeals;

namespace WebApiDynamodbLocal.Services.BigTimeDeals.Interfaces;

public interface IBrandService
{
    Task<ResponseBaseDto> CreateAsync(Brand brand);
    Task<GetResponseDto> GetAsync(string name);
}
