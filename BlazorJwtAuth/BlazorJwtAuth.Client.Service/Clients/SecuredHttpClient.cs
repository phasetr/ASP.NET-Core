using System.Net;
using System.Net.Http.Headers;
using BlazorJwtAuth.Client.Common.Library;
using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Clients;

public class SecuredHttpClient
{
    private readonly HttpClient _http;

    public SecuredHttpClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<SecuredDataResultDto> GetSecuredDataAsync(AppSettings appSettings, string token)
    {
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _http.GetAsync($"{appSettings.ApiBaseAddress}/Secured");
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
}
