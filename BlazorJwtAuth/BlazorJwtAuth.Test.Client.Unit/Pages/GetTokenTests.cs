using System;
using System.Collections.Generic;
using System.Net;
using BlazorJwtAuth.Client.Common.Library;
using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.Common.Models;
using RichardSzalay.MockHttp;

namespace BlazorJwtAuth.Test.Client.Unit.Pages;

public class GetTokenTests : TestContext
{
    [Fact]
    public void GetTokenAsync_Authenticated()
    {
        var mockAppSettings = new AppSettings
        {
            ApiBaseAddress = "https://localhost:5001"
        };
        Services.AddSingleton(mockAppSettings);
        var mockHttpClient = Services.AddMockHttpClient();
        mockHttpClient.When($"{mockAppSettings.ApiBaseAddress}/User/token").RespondJson(new AuthenticationResponse
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
        mockHttpClient.When($"{mockAppSettings.ApiBaseAddress}/Secured").RespondJson(new SecuredDataResultDto
        {
            Detail = "detail",
            Message = "message",
            Status = HttpStatusCode.OK.ToString()
        });

        /*
        var cut = RenderComponent<GetToken>();
        cut.Find("#apiBaseAddress")
            .MarkupMatches("""<input id="apiBaseAddress" class="col-8" value="https://localhost:5001"/>""");
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
        */
    }
}
