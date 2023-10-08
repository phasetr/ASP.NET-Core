using Common.Dto;

namespace Client.Service.Services.Interfaces;

public interface ISecuredHttpClientService
{
    Task<SecuredDataResponseDto> GetSecuredDataAsync(HttpClient httpClient, string token);
}
