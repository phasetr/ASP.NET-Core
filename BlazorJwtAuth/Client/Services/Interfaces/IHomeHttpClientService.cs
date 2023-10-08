using Common.Dto;

namespace Client.Services.Interfaces;

public interface IHomeHttpClientService
{
    Task<ResponseBaseDto> GetIndexAsync(HttpClient httpClient);
}
