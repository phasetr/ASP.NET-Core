using Common.Dto;
using WebApiDynamodbLocal.Dto.SessionStore;
using ResponseBaseDto = WebApiDynamodbLocal.Dto.ResponseBaseDto;

namespace WebApiDynamodbLocal.Services.SessionStore.interfaces;

public interface ISessionService
{
    Task<ResponseBaseWithKeyDto> CreateAsync(PostDto dto);
    Task<GetResponseDto> GetAsync(string sessionId);
    Task<ResponseBaseDto> DeleteByUserNameAsync(string userName);
}
