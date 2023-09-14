using System.Net.Http.Json;
using BlazorJwtAuth.Client.Service.Services.Interfaces;
using BlazorJwtAuth.Common.Constants;
using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services;

public class HomeHttpClientService : IHomeHttpClientService
{
    public async Task<ResponseBaseDto> GetIndexAsync(HttpClient httpClient)
    {
        try
        {
            var response = await httpClient.GetAsync(ApiPath.V1Home);
            var result = await response.Content.ReadFromJsonAsync<ResponseBaseDto>();
            if (result is null)
                return new ResponseBaseDto
                {
                    Message = "Response is null"
                };

            return new ResponseBaseDto
            {
                Message = result.Message
            };
        }
        catch (Exception ex)
        {
            return new ResponseBaseDto
            {
                Message = "Sorry, some problem occurred. Please try again."
            };
        }
    }
}
