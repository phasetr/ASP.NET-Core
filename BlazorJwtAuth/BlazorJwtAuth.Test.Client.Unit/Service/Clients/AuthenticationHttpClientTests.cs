using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorJwtAuth.Client.Service.Classes;
using BlazorJwtAuth.Client.Service.Clients;
using BlazorJwtAuth.Client.Service.Services;
using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.Test.Client.Unit.Helpers;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace BlazorJwtAuth.Test.Client.Unit.Service.Clients;

public class AuthenticationHttpClientTests : TestContext
{
    [Fact]
    public async Task RegisterUser_Success()
    {
        var userRegisterDto = new UserRegisterDto
        {
            Email = "email@phasetr.com",
            Password = "password",
            ConfirmPassword = "password"
        };
        var userRegisterResultDto = new UserRegisterResultDto
        {
            Detail = "detail",
            Errors = new List<string>(),
            Message = "message",
            Succeeded = true,
            Status = HttpStatusCode.OK.ToString()
        };

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{Constants.AppSettings.ApiBaseAddress}/User/register")
            .Respond("application/json", JsonSerializer.Serialize(userRegisterResultDto));
        var mockHttpClient = mockHttp.ToHttpClient();
        var mockLocalStorage = Substitute.For<ILocalStorageService>();
        mockLocalStorage.GetItemAsync<TokenDto>(Arg.Any<string>()).Returns(new TokenDto
        {
            Token = "token",
            Expiration = DateTime.UtcNow.AddDays(1)
        });
        var tokenService = new TokenService(mockLocalStorage);
        var customAuthenticationStateProvider = new CustomAuthenticationStateProvider(tokenService);
        var authenticationHttpClient = new AuthenticationHttpClient(
            customAuthenticationStateProvider,
            mockHttpClient,
            tokenService);

        var result = await authenticationHttpClient.RegisterUser(userRegisterDto, Constants.AppSettings);

        Assert.NotNull(result);
        Assert.True(result.Succeeded);
        Assert.Equal(HttpStatusCode.OK.ToString(), result.Status);
    }

    [Fact]
    public async Task RegisterUser_Failed()
    {
        var userRegisterDto = new UserRegisterDto
        {
            Email = "email@phasetr.com",
            Password = "password",
            ConfirmPassword = "password"
        };
        UserRegisterResultDto? userRegisterResultDto = null;

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{Constants.AppSettings.ApiBaseAddress}/User/register")
            .Respond("application/json", JsonSerializer.Serialize(userRegisterResultDto));
        var mockHttpClient = mockHttp.ToHttpClient();
        var mockLocalStorage = Substitute.For<ILocalStorageService>();
        mockLocalStorage.GetItemAsync<TokenDto>(Arg.Any<string>()).Returns(new TokenDto
        {
            Token = "token",
            Expiration = DateTime.UtcNow.AddDays(1)
        });
        var tokenService = new TokenService(mockLocalStorage);
        var customAuthenticationStateProvider = new CustomAuthenticationStateProvider(tokenService);
        var authenticationHttpClient = new AuthenticationHttpClient(
            customAuthenticationStateProvider,
            mockHttpClient,
            tokenService);

        var result = await authenticationHttpClient.RegisterUser(userRegisterDto, Constants.AppSettings);

        Assert.NotNull(result);
        Assert.Equal("Unable to deserialize response from server.", result.Detail);
        Assert.Equal("Sorry, we were unable to register you at this time. Please try again shortly.",
            result.Errors.ToList()[0]);
        Assert.Equal("Sorry, we were unable to register you at this time. Please try again shortly.", result.Message);
        Assert.Equal(HttpStatusCode.BadRequest.ToString(), result.Status);
        Assert.False(result.Succeeded);
    }

    [Fact]
    public async Task LoginUser_Success()
    {
        var userLoginDto = new UserLoginDto
        {
            Email = "user@secureapi.com",
            Password = "Pa$$w0rd."
        };
        var tokenDto = new TokenDto()
        {
            Token = "token",
            Expiration = DateTime.UtcNow.AddDays(1)
        };
        var userLoginResultDto = new UserLoginResultDto()
        {
            Detail = "detail",
            Message = "message",
            Succeeded = true,
            Status = HttpStatusCode.OK.ToString(),
            Token = tokenDto
        };

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{Constants.AppSettings.ApiBaseAddress}/User/login")
            .Respond("application/json", JsonSerializer.Serialize(userLoginResultDto));
        var mockHttpClient = mockHttp.ToHttpClient();
        var mockLocalStorage = Substitute.For<ILocalStorageService>();
        mockLocalStorage.GetItemAsync<TokenDto>(Arg.Any<string>()).Returns(tokenDto);
        var tokenService = new TokenService(mockLocalStorage);
        var customAuthenticationStateProvider = new CustomAuthenticationStateProvider(tokenService);
        var authenticationHttpClient = new AuthenticationHttpClient(
            customAuthenticationStateProvider,
            mockHttpClient,
            tokenService);

        var result = await authenticationHttpClient.LoginUser(userLoginDto, Constants.AppSettings);

        Assert.NotNull(result);
        Assert.Equal("detail", result.Detail);
        Assert.Equal("message", result.Message);
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task LoginUser_Failed()
    {
        var userLoginDto = new UserLoginDto
        {
            Email = "user@secureapi.com",
            Password = "Pa$$w0rd."
        };
        var tokenDto = new TokenDto()
        {
            Token = "token",
            Expiration = DateTime.UtcNow.AddDays(1)
        };
        UserLoginResultDto? userLoginResultDto = null;

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{Constants.AppSettings.ApiBaseAddress}/User/login")
            .Respond("application/json", JsonSerializer.Serialize(userLoginResultDto));
        var mockHttpClient = mockHttp.ToHttpClient();
        var mockLocalStorage = Substitute.For<ILocalStorageService>();
        mockLocalStorage.GetItemAsync<TokenDto>(Arg.Any<string>()).Returns(tokenDto);
        var tokenService = new TokenService(mockLocalStorage);
        var customAuthenticationStateProvider = new CustomAuthenticationStateProvider(tokenService);
        var authenticationHttpClient = new AuthenticationHttpClient(
            customAuthenticationStateProvider,
            mockHttpClient,
            tokenService);

        var result = await authenticationHttpClient.LoginUser(userLoginDto, Constants.AppSettings);

        Assert.NotNull(result);
        Assert.Equal("Unable to deserialize response from server.", result.Detail);
        Assert.Equal("Sorry, we were unable to register you at this time. Please try again shortly.", result.Message);
        Assert.False(result.Succeeded);
    }
}
