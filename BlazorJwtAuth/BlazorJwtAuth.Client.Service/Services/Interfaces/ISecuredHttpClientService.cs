using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services.Interfaces;

public interface ISecuredHttpClientService
{
    Task<SecuredDataResponseDto> GetSecuredDataAsync(HttpClient httpClient, string token);
}
