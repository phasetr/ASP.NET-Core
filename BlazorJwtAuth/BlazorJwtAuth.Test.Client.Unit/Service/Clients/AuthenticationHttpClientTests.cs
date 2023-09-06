using System;
using System.Collections.Generic;
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
}
