using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
        mockHttpClient.When($"{Constants.AppSettings.ApiBaseAddress}/User/token").RespondJson(new AuthenticationResponse
        {
            IsAuthenticated = true,
            Detail = "detail",
            Email = "user@securityapit.com",
            Message = "message",
            RefreshToken = "refreshToken",
            RefreshTokenExpiration = DateTime.Now.AddDays(1),
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
        Services.AddHttpClient<HttpClient>(client =>
            client.BaseAddress = new Uri(Constants.AppSettings.ApiBaseAddress));
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
                    <input id="token">
                </dd>
                <dt>message</dt>
                <dd></dd>
                <dt>is authenticated</dt>
                <dd></dd>
                <dt>user name</dt>
                <dd></dd>
                <dt>refresh token</dt>
                <dd>
                    <input id="refreshToken">
                </dd>
                <dt>refresh token expiration</dt>
                <dd></dd>
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
        Services.AddHttpClient<HttpClient>(client =>
            client.BaseAddress = new Uri(Constants.AppSettings.ApiBaseAddress));
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
            Detail = "detail",
            Message = "message",
            Status = HttpStatusCode.OK.ToString()
        });
        Services.AddHttpClient<HttpClient>(client =>
            client.BaseAddress = new Uri(Constants.AppSettings.ApiBaseAddress));
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
            .MarkupMatches("""<dd id="secureDataMessage"></dd>""");
    }
}
