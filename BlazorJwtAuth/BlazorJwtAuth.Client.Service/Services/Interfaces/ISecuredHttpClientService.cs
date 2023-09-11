using BlazorJwtAuth.Client.Service.Helpers;
using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services.Interfaces;

public interface ISecuredHttpClientService
{
    Task<SecuredDataResponseDto> GetSecuredDataAsync(AppSettings appSettings, HttpClient httpClient, string token);
}
