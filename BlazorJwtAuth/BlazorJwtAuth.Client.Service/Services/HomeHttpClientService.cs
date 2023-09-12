using System.Net;
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
                    Detail = "",
                    Message = "Response is null",
                    Status = HttpStatusCode.InternalServerError.ToString()
                };

            return new ResponseBaseDto
            {
                Detail = result.Detail,
                Message = result.Message,
                Status = result.Status
            };
        }
        catch (Exception ex)
        {
            return new ResponseBaseDto
            {
                Detail = ex.Message,
                Message = "Sorry, some problem occurred. Please try again.",
                Status = HttpStatusCode.InternalServerError.ToString()
            };
        }
    }
}
