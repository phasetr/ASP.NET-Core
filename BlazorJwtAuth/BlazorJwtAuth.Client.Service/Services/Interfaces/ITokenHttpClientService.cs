using BlazorJwtAuth.Client.Service.Helpers;
using BlazorJwtAuth.Common.Models;

namespace BlazorJwtAuth.Client.Service.Services.Interfaces;

public interface ITokenHttpClientService
{
    Task<AuthenticationResponse> GetTokenAsync(AppSettings appSettings, GetTokenRequest getTokenRequest);
    Task<AuthenticationResponse> RefreshTokenAsync(AppSettings appSettings, RefreshTokenRequest refreshTokenRequest);
}
