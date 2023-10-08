using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Client.Services;
using Common.Constants;
using Common.Dto;
using Common.Services;
using RichardSzalay.MockHttp;
using Test.Client.Unit.Helpers;

namespace Test.Client.Unit.Service.Services;

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
                Email = "user@secureapi.com",
                IsAuthenticated = true,
                Message = "message",
                RefreshToken = "refresh-token",
                RefreshTokenExpiration = dateTime.AddDays(1),
                Roles = new List<string> {"role1", "role2"},
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
        Assert.Equal("user@secureapi.com", result.Email);
        Assert.True(result.IsAuthenticated);
        Assert.Equal("message", result.Message);
        Assert.Equal("refresh-token", result.RefreshToken);
        Assert.True(result.RefreshTokenExpiration > dateTime);
        Assert.Equal("role1", result.Roles[0]);
        Assert.Equal("role2", result.Roles[1]);
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
                Email = "user@secureapi.com",
                IsAuthenticated = true,
                Message = "message",
                RefreshToken = "refresh-token",
                RefreshTokenExpiration = dateTime.AddDays(1),
                Roles = new List<string> {"role1", "role2"},
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
        Assert.Equal("user@secureapi.com", result.Email);
        Assert.True(result.IsAuthenticated);
        Assert.Equal("message", result.Message);
        Assert.Equal("refresh-token", result.RefreshToken);
        Assert.True(result.RefreshTokenExpiration > dateTime);
        Assert.Equal("role1", result.Roles[0]);
        Assert.Equal("role2", result.Roles[1]);
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
        Assert.False(result.IsAuthenticated);
        Assert.Equal("Sorry, we were unable to refresh a token. Please try again shortly.", result.Message);
    }
}
