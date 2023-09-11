using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BlazorJwtAuth.Client.Service.Helpers;
using BlazorJwtAuth.Client.Service.Services.Interfaces;
using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services;

public class SecuredHttpClientService : ISecuredHttpClientService
{
    public async Task<SecuredDataResultDto> GetSecuredDataAsync(
        AppSettings appSettings,
        HttpClient httpClient,
        string token)
    {
        try
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.GetAsync($"{appSettings.ApiBaseAddress}/Secured");
            var result = await response.Content.ReadFromJsonAsync<SecuredDataResultDto>();
            return new SecuredDataResultDto
            {
                Message = response.StatusCode switch
                {
                    HttpStatusCode.Unauthorized => "Unauthorized: Please check your token.",
                    HttpStatusCode.OK => result!.Message,
                    _ => "Oops! Something went wrong."
                },
                Status = response.StatusCode.ToString()
            };
        }
        catch (Exception ex)
        {
            return new SecuredDataResultDto
            {
                Message = ex.Message,
                Status = HttpStatusCode.InternalServerError.ToString()
            };
        }
    }
}
