using BlazorJwtAuth.Client.Service.Services.Interfaces;
using BlazorJwtAuth.Common.Models;

namespace BlazorJwtAuth.Client.Service.Services;

public class TokenService : ITokenService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TokenService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<AuthenticationResponse> GetTokenAsync(GetTokenRequest tokenRequest)
    {
        throw new NotImplementedException();
        // var httpClient = _httpClientFactory.CreateClient();
        // // TODO APIのURLルート取得
        // var response =
        //     await httpClient.PostAsync("https://localhost:5500/User/token", JsonContent.Create(tokenRequest));
        // var res = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();
        // if (res is null)
        //     return new AuthenticationResponse
        //     {
        //         Status = Response.ErrorResponseStatus,
        //         Message = Response.ErrorResponseMessage,
        //         Detail = Response.ErrorResponseDetail
        //     };
        // return res;
    }

    public async Task<AuthenticationResponse> RefreshTokenAsync(string token, string refreshToken)
    {
        throw new NotImplementedException();
    }
}
