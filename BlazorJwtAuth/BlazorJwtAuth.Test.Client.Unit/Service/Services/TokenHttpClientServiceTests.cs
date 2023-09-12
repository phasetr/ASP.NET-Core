using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorJwtAuth.Client.Service.Services;
using BlazorJwtAuth.Common.Constants;
using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.Common.Services;
using BlazorJwtAuth.Test.Client.Unit.Helpers;
using RichardSzalay.MockHttp;

namespace BlazorJwtAuth.Test.Client.Unit.Service.Services;

public class TokenHttpClientServiceTests
{
    private readonly PtDateTime _ptDateTime = new();
    private TokenHttpClientService _sut = default!;

    [Fact]
    public async Task GetTokenAsync_Success()
    {
        var dateTime = _ptDateTime.UtcNow;
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{Constants.AppSettings.ApiBaseAddress}/{ApiPath.V1UserGetTokenFull}")
            .Respond("application/json", JsonSerializer.Serialize(new AuthenticationResponseDto
            {
                Detail = "detail",
                Email = "user@secureapi.com",
                IsAuthenticated = true,
                Message = "message",
                RefreshToken = "refresh-token",
                RefreshTokenExpiration = dateTime.AddDays(1),
                Roles = new List<string> {"role1", "role2"},
                Status = HttpStatusCode.OK.ToString(),
                Token = "token",
                UserName = "user"
            }));
        var mockHttpClient = mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = new Uri(Constants.AppSettings.ApiBaseAddress);

        _sut = new TokenHttpClientService();
        var result = await _sut.GetTokenAsync(mockHttpClient, new GetTokenResponseDto
        {
            Email = "user@secureapi.com",
            Password = "Pa$$w0rd."
        });

        Assert.NotNull(result);
        Assert.Equal("detail", result.Detail);
        Assert.Equal("user@secureapi.com", result.Email);
        Assert.True(result.IsAuthenticated);
        Assert.Equal("message", result.Message);
        Assert.Equal("refresh-token", result.RefreshToken);
        Assert.True(result.RefreshTokenExpiration > dateTime);
        Assert.Equal("role1", result.Roles[0]);
        Assert.Equal("role2", result.Roles[1]);
        Assert.Equal(HttpStatusCode.OK.ToString(), result.Status);
        Assert.Equal("token", result.Token);
        Assert.Equal("user", result.UserName);
    }

    [Fact]
    public async Task GetTokenAsync_Failed()
    {
        var mockHttp = new MockHttpMessageHandler();
        AuthenticationResponseDto? authenticationResponse = null;
        mockHttp.When($"{Constants.AppSettings.ApiBaseAddress}/{ApiPath.V1UserGetTokenFull}")
            .Respond("application/json", JsonSerializer.Serialize(authenticationResponse));
        var mockHttpClient = mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = new Uri(Constants.AppSettings.ApiBaseAddress);

        _sut = new TokenHttpClientService();
        var result = await _sut.GetTokenAsync(mockHttpClient, new GetTokenResponseDto
        {
            Email = "user@secureapi.com",
            Password = "Pa$$w0rd."
        });

        Assert.NotNull(result);
        Assert.Equal("Unable to deserialize response from server.", result.Detail);
        Assert.False(result.IsAuthenticated);
        Assert.Equal("Sorry, we were unable to authenticate you at this time. Please try again shortly.",
            result.Message);
    }

    [Fact]
    public async Task RefreshTokenAsync_Success()
    {
        var dateTime = _ptDateTime.UtcNow;
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{Constants.AppSettings.ApiBaseAddress}/{ApiPath.V1UserRefreshTokenFull}")
            .Respond("application/json", JsonSerializer.Serialize(new AuthenticationResponseDto
            {
                Detail = "detail",
                Email = "user@secureapi.com",
                IsAuthenticated = true,
                Message = "message",
                RefreshToken = "refresh-token",
                RefreshTokenExpiration = dateTime.AddDays(1),
                Roles = new List<string> {"role1", "role2"},
                Status = HttpStatusCode.OK.ToString(),
                Token = "token",
                UserName = "user"
            }));
        var mockHttpClient = mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = new Uri(Constants.AppSettings.ApiBaseAddress);

        _sut = new TokenHttpClientService();
        var result = await _sut.RefreshTokenAsync(
            mockHttpClient,
            new RefreshTokenDto
            {
                RefreshToken = "refresh-token"
            });

        Assert.NotNull(result);
        Assert.Equal("detail", result.Detail);
        Assert.Equal("user@secureapi.com", result.Email);
        Assert.True(result.IsAuthenticated);
        Assert.Equal("message", result.Message);
        Assert.Equal("refresh-token", result.RefreshToken);
        Assert.True(result.RefreshTokenExpiration > dateTime);
        Assert.Equal("role1", result.Roles[0]);
        Assert.Equal("role2", result.Roles[1]);
        Assert.Equal(HttpStatusCode.OK.ToString(), result.Status);
        Assert.Equal("token", result.Token);
        Assert.Equal("user", result.UserName);
    }

    [Fact]
    public async Task RefreshTokenAsync_Failed()
    {
        var mockHttp = new MockHttpMessageHandler();
        AuthenticationResponseDto? authenticationResponse = null;
        mockHttp.When($"{Constants.AppSettings.ApiBaseAddress}/{ApiPath.V1UserRefreshTokenFull}")
            .Respond("application/json", JsonSerializer.Serialize(authenticationResponse));
        var mockHttpClient = mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = new Uri(Constants.AppSettings.ApiBaseAddress);

        _sut = new TokenHttpClientService();
        var result = await _sut.RefreshTokenAsync(
            mockHttpClient,
            new RefreshTokenDto
            {
                RefreshToken = "refresh-token"
            });

        Assert.NotNull(result);
        Assert.Equal("Unable to deserialize response from server.", result.Detail);
        Assert.False(result.IsAuthenticated);
        Assert.Equal("Sorry, we were unable to refresh a token. Please try again shortly.", result.Message);
    }
}
