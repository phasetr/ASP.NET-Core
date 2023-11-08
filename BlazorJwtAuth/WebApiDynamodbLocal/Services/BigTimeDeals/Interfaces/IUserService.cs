using WebApiDynamodbLocal.Dto;
using WebApiDynamodbLocal.Dto.BigTimeDeals.User;
using WebApiDynamodbLocal.Entities.BigTimeDeals;

namespace WebApiDynamodbLocal.Services.BigTimeDeals.Interfaces;

public interface IUserService
{
    Task<ResponseBaseDto> CreateAsync(User user);
    Task<GetResponseDto> GetAsync(string userName);
}
