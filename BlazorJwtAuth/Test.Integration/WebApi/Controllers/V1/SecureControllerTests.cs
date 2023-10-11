using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Common.Constants;
using Common.Dto;
using Microsoft.AspNetCore.Mvc.Testing;
using Authorization = Common.Constants.Authorization;

namespace Test.Integration.WebApi.Controllers.V1;

public class SecureControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public SecureControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetSecuredData_WhenCalled_ReturnsOk()
    {
        // HTTPクライアントの初期化
        var client = _factory.CreateClient();

        // APIでトークンを取得
        var getTokenDto = new GetTokenDto
        {
            Email = "user@secureapi.com",
            Password = "Pa$$w0rd."
        };
        var json = JsonSerializer.Serialize(getTokenDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var tokenResponse = await client.PostAsync(ApiPath.V1UserGetTokenFull, content);
        var token = await tokenResponse.Content.ReadFromJsonAsync<AuthenticationResponseDto>();
        Assert.NotNull(token);

        // APIにトークンを付与してリクエスト
        // トークンはヘッダーからクッキーに変更
        // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        var cookie = new Cookie(Authorization.JwtAccessTokenName, token.Token);
        client.DefaultRequestHeaders.Add("Cookie", cookie.ToString());
        var response = await client.GetAsync("api/v1/Secured");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<SecuredDataResponseDto>();

        // レスポンスを確認
        Assert.NotNull(result);
        Assert.Equal("This Secured Data is available only for Authenticated Users.", result.Message);
    }

    [Fact]
    public async Task PostSecuredData_WhenCalled_ReturnsOk()
    {
        // HTTPクライアントの初期化
        var client = _factory.CreateClient();

        // APIでトークンを取得
        var getTokenDto = new GetTokenDto
        {
            Email = "admin@secureapi.com",
            Password = "adminpass"
        };
        var json = JsonSerializer.Serialize(getTokenDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var tokenResponse = await client.PostAsync("/api/v1/User/token", content);
        var token = await tokenResponse.Content.ReadFromJsonAsync<AuthenticationResponseDto>();
        Assert.NotNull(token);

        // APIにトークンを付与してPOSTリクエスト
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        var response = await client.PostAsync("api/v1/Secured", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<SecuredDataResponseDto>();

        // レスポンスを確認
        Assert.NotNull(result);
        Assert.Equal("This Secured Data is available only for Administrators.", result.Message);
    }
}
