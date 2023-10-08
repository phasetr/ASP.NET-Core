using System.Net.Http.Json;
using Client.Services.Interfaces;
using Common.Constants;
using Common.Dto;

namespace Client.Services;

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
                Errors = new List<string> {ex.Message},
                Succeeded = false,
                Message = "Sorry, some problem occurred. Please try again."
            };
        }
    }
}
