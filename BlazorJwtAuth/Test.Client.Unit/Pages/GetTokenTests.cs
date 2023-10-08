using System;
using System.Collections.Generic;
using Client.Pages;
using Client.Services;
using Client.Services.Interfaces;
using Common.Constants;
using Common.Dto;
using RichardSzalay.MockHttp;
using Test.Client.Unit.Helpers;

namespace Test.Client.Unit.Pages;

public class GetTokenTests : TestContext
{
    [Fact]
    public void GetTokenAsync_Authenticated()
    {
        Services.AddSingleton(Constants.AppSettings);
        var mockHttpClient = Services.AddMockHttpClient();
        var dateTime = new DateTime(2023, 1, 1, 0, 0, 0);
        mockHttpClient.When($"{Constants.AppSettings.ApiBaseAddress}/{ApiPath.V1UserGetTokenFull}").RespondJson(
            new AuthenticationResponseDto
            {
                IsAuthenticated = true,
                Email = "user@secureapi.com",
                Message = "message",
                RefreshToken = "refreshToken",
                RefreshTokenExpiration = dateTime.AddDays(1),
                Roles = new List<string> {"Moderator"},
                Token = "token",
                UserName = "user"
            });
        mockHttpClient.When($"{Constants.AppSettings.ApiBaseAddress}/{ApiPath.V1Secured}").RespondJson(
            new SecuredDataResponseDto
            {
                Message = "message"
            });
        Services.AddScoped<ISecuredHttpClientService, SecuredHttpClientService>();
        Services.AddScoped<ITokenHttpClientService, TokenHttpClientService>();

        var cut = RenderComponent<GetToken>();
        cut.Find("#apiBaseAddress")
            .MarkupMatches("""<input id="apiBaseAddress" class="col-8" value="http://localhost"/>""");
        cut.Find("#getTokenAsync")
            .MarkupMatches("""<button id="getTokenAsync" class="btn btn-outline-primary">Get Token</button>""");
        cut.Find("#getTokenAsync").Click();
        cut.WaitForElement("#getTokenResult").MarkupMatches(
            """
            <dl id="getTokenResult">
                <dt>token</dt>
                <dd>
                    <input id="token" value="token">
                </dd>
                <dt>message</dt>
                <dd>message</dd>
                <dt>is authenticated</dt>
                <dd>True</dd>
                <dt>user name</dt>
                <dd>user</dd>
                <dt>refresh token</dt>
                <dd>
                    <input id="refreshToken" value="refreshToken">
                </dd>
                <dt>refresh token expiration</dt>
                <dd>2023/01/02 9:00:00</dd>
            </dl>
            """);
    }

    [Fact]
    public void GetSecuredDataAsync_NoToken()
    {
        Services.AddSingleton(Constants.AppSettings);
        var mockHttpClient = Services.AddMockHttpClient();
        mockHttpClient.When($"{Constants.AppSettings.ApiBaseAddress}/{ApiPath.V1Secured}").RespondJson(
            new SecuredDataResponseDto
            {
                Message = "message"
            });
        Services.AddScoped<ISecuredHttpClientService, SecuredHttpClientService>();
        Services.AddScoped<ITokenHttpClientService, TokenHttpClientService>();

        var cut = RenderComponent<GetToken>();
        cut.Find("#apiBaseAddress")
            .MarkupMatches("""<input id="apiBaseAddress" class="col-8" value="http://localhost"/>""");
        cut.Find("#getSecureDataAsync")
            .MarkupMatches(
                """<button id="getSecureDataAsync" class="btn btn-outline-primary">Get Secure Data</button>""");
        cut.Find("#getSecureDataAsync").Click();
        cut.WaitForElement("#secureDataMessage")
            .MarkupMatches("""<dd id="secureDataMessage">You need to assign a token!</dd>""");
    }

    [Fact]
    public void GetSecuredDataAsync_WithProperToken()
    {
        Services.AddSingleton(Constants.AppSettings);
        var mockHttpClient = Services.AddMockHttpClient();
        mockHttpClient.When($"{Constants.AppSettings.ApiBaseAddress}/{ApiPath.V1Secured}").RespondJson(
            new SecuredDataResponseDto
            {
                Message = "message"
            });
        Services.AddScoped<ISecuredHttpClientService, SecuredHttpClientService>();
        Services.AddScoped<ITokenHttpClientService, TokenHttpClientService>();

        var cut = RenderComponent<GetToken>(parameters =>
            parameters.Add(p => p.Token, "token"));
        cut.Find("#apiBaseAddress")
            .MarkupMatches("""<input id="apiBaseAddress" class="col-8" value="http://localhost"/>""");
        cut.Find("#getSecureDataAsync")
            .MarkupMatches(
                """<button id="getSecureDataAsync" class="btn btn-outline-primary">Get Secure Data</button>""");
        cut.Find("#getSecureDataAsync").Click();
        cut.WaitForElement("#secureDataMessage")
            .MarkupMatches("""<dd id="secureDataMessage">message</dd>""");
    }
}
