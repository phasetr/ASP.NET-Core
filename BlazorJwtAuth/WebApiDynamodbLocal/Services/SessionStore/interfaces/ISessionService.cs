using Common.Dto;
using WebApiDynamodbLocal.Dto.SessionStore;

namespace WebApiDynamodbLocal.Services.SessionStore.interfaces;

public interface ISessionService
{
    Task<PostResponseDto> CreateAsync(PostDto dto);
    Task<GetResponseDto> GetAsync(string sessionId);
    Task<ResponseBaseDto> DeleteByUserNameAsync(string userName);
}
