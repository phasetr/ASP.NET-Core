using System.Net;
using System.Net.Http.Json;
using BlazorJwtAuth.Client.Service.Helpers;
using BlazorJwtAuth.Client.Service.Services.Interfaces;
using BlazorJwtAuth.Common.Models;

namespace BlazorJwtAuth.Client.Service.Services;

public class HomeHttpClientService : IHomeHttpClientService
{
    public async Task<ResponseBase> GetIndexAsync(AppSettings appSettings, HttpClient httpClient)
    {
        try
        {
            var response = await httpClient.GetAsync(appSettings.ApiBaseAddress);
            var result = await response.Content.ReadFromJsonAsync<ResponseBase>();
            if (result is null)
                return new ResponseBase
                {
                    Detail = "",
                    Message = "Response is null",
                    Status = HttpStatusCode.InternalServerError.ToString()
                };

            return new ResponseBase
            {
                Detail = result.Detail,
                Message = result.Message,
                Status = result.Status
            };
        }
        catch (Exception ex)
        {
            return new ResponseBase
            {
                Detail = ex.Message,
                Message = "Sorry, some problem occurred. Please try again.",
                Status = HttpStatusCode.InternalServerError.ToString()
            };
        }
    }
}
