using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BlazorJwtAuth.Client.Service.Services.Interfaces;
using BlazorJwtAuth.Common.Constants;
using BlazorJwtAuth.Common.Dto;

namespace BlazorJwtAuth.Client.Service.Services;

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
