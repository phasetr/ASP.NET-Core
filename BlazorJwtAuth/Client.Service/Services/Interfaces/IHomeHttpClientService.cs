using Common.Dto;

namespace Client.Service.Services.Interfaces;

public interface IHomeHttpClientService
{
    Task<ResponseBaseDto> GetIndexAsync(HttpClient httpClient);
}
