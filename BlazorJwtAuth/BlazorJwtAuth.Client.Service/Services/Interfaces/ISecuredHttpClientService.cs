using BlazorJwtAuth.Client.Common.Library;
using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services.Interfaces;

public interface ISecuredHttpClientService
{
    Task<SecuredDataResultDto> GetSecuredDataAsync(AppSettings appSettings, string token);
}
