using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services.Interfaces;

public interface IHomeHttpClientService
{
    Task<ResponseBaseDto> GetIndexAsync(HttpClient httpClient);
}
