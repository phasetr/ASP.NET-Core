using BlazorJwtAuth.Client.Service.Helpers;
using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services.Interfaces;

public interface ISecuredHttpClientService
{
    Task<SecuredDataResultDto> GetSecuredDataAsync(AppSettings appSettings, HttpClient httpClient, string token);
}
