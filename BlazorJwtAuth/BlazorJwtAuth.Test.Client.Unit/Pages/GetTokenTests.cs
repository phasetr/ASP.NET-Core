using System;
using System.Collections.Generic;
using System.Net;
using BlazorJwtAuth.Client.Pages;
using BlazorJwtAuth.Client.Service.Services;
using BlazorJwtAuth.Client.Service.Services.Interfaces;
using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.Common.Models;
using BlazorJwtAuth.Test.Client.Unit.Helpers;
using RichardSzalay.MockHttp;

namespace BlazorJwtAuth.Test.Client.Unit.Pages;

public class GetTokenTests : TestContext
{
    [Fact]
    public void GetTokenAsync_Authenticated()
    {
        Services.AddSingleton(Constants.AppSettings);
        var mockHttpClient = Services.AddMockHttpClient();
        var dateTime = new DateTime(2023, 1, 1, 0, 0, 0);
        mockHttpClient.When($"{Constants.AppSettings.ApiBaseAddress}/User/token").RespondJson(new AuthenticationResponse
        {
            IsAuthenticated = true,
            Detail = "detail",
            Email = "user@secureapi.com",
            Message = "message",
            RefreshToken = "refreshToken",
            RefreshTokenExpiration = dateTime.AddDays(1),
            Roles = new List<string> {"Moderator"},
            Status = HttpStatusCode.OK.ToString(),
            Token = "token",
            UserName = "user"
        });
        mockHttpClient.When($"{Constants.AppSettings.ApiBaseAddress}/Secured").RespondJson(new SecuredDataResultDto
        {
            Detail = "detail",
            Message = "message",
            Status = HttpStatusCode.OK.ToString()
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
        mockHttpClient.When($"{Constants.AppSettings.ApiBaseAddress}/Secured").RespondJson(new SecuredDataResultDto
        {
            Detail = "detail",
            Message = "message",
            Status = HttpStatusCode.OK.ToString()
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
        mockHttpClient.When($"{Constants.AppSettings.ApiBaseAddress}/Secured").RespondJson(new SecuredDataResultDto
        {
            Detail = "",
            Message = "message",
            Status = HttpStatusCode.OK.ToString()
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
