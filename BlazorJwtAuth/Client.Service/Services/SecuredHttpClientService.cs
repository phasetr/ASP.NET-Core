using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Client.Service.Services.Interfaces;
using Common.Constants;
using Common.Dto;

namespace Client.Service.Services;

public class SecuredHttpClientService : ISecuredHttpClientService
{
    public async Task<SecuredDataResponseDto> GetSecuredDataAsync(HttpClient httpClient, string token)
    {
        try
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.GetAsync(ApiPath.V1Secured);
            var result = await response.Content.ReadFromJsonAsync<SecuredDataResponseDto>();
            return new SecuredDataResponseDto
            {
                Message = response.StatusCode switch
                {
                    HttpStatusCode.Unauthorized => "Unauthorized: Please check your token.",
                    HttpStatusCode.OK => result!.Message,
                    _ => "Oops! Something went wrong."
                }
            };
        }
        catch (Exception ex)
        {
            return new SecuredDataResponseDto
            {
                Message = ex.Message
            };
        }
    }
}
