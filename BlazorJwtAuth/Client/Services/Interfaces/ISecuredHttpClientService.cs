using Common.Dto;

namespace Client.Services.Interfaces;

public interface ISecuredHttpClientService
{
    Task<SecuredDataResponseDto> GetSecuredDataAsync(HttpClient httpClient, string token);
}
