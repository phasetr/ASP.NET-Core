using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BlazorJwtAuth.Common.Dto;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BlazorJwtAuth.Test.Integration.WebApi.Controllers.V1;

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
        var getTokenRequest = new GetTokenResponseDto
        {
            Email = "user@secureapi.com",
            Password = "Pa$$w0rd."
        };
        var json = JsonSerializer.Serialize(getTokenRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var tokenResponse = await client.PostAsync("/api/v1/User/token", content);
        var token = await tokenResponse.Content.ReadFromJsonAsync<AuthenticationResponseDto>();
        Assert.NotNull(token);

        // APIにトークンを付与してリクエスト
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        var response = await client.GetAsync("api/v1/Secured");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<SecuredDataResponseDto>();

        // レスポンスを確認
        Assert.NotNull(result);
        Assert.Null(result.Detail);
        Assert.Equal("This Secured Data is available only for Authenticated Users.", result.Message);
        Assert.Equal(HttpStatusCode.OK.ToString(), result.Status);
    }

    [Fact]
    public async Task PostSecuredData_WhenCalled_ReturnsOk()
    {
        // HTTPクライアントの初期化
        var client = _factory.CreateClient();

        // APIでトークンを取得
        var getTokenRequest = new GetTokenResponseDto
        {
            Email = "admin@secureapi.com",
            Password = "adminpass"
        };
        var json = JsonSerializer.Serialize(getTokenRequest);
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
        Assert.Null(result.Detail);
        Assert.Equal("This Secured Data is available only for Administrators.", result.Message);
        Assert.Equal(HttpStatusCode.OK.ToString(), result.Status);
    }
}
