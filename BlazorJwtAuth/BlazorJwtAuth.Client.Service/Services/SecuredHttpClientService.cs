using System.Net;
using System.Net.Http.Headers;
using BlazorJwtAuth.Client.Service.Helpers;
using BlazorJwtAuth.Client.Service.Services.Interfaces;
using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services;

public class SecuredHttpClientService : ISecuredHttpClientService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SecuredHttpClientService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<SecuredDataResultDto> GetSecuredDataAsync(AppSettings appSettings, string token)
    {
        try
        {
            var http = _httpClientFactory.CreateClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await http.GetAsync($"{appSettings.ApiBaseAddress}/Secured");
            return new SecuredDataResultDto
            {
                Message = response.StatusCode switch
                {
                    HttpStatusCode.Unauthorized => "Unauthorized: Please check your token.",
                    HttpStatusCode.OK => await response.Content.ReadAsStringAsync(),
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
