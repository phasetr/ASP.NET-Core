using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BlazorJwtAuth.Common.Constants;
using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BlazorJwtAuth.Test.Integration.WebApi.Controllers.V1;

public class WeatherForecastControllerTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public WeatherForecastControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_WhenNoAuthCalled_Returns401()
    {
        // HTTPクライアントの初期化
        var client = _factory.CreateClient();

        // APIにトークンを付与してリクエスト
        var response = await client.GetAsync(ApiPath.V1WeatherForecastFull);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_WhenAuthCalled_ReturnsOk()
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
        var tokenResponse = await client.PostAsync("/api/v1/user/token", content);
        var token = await tokenResponse.Content.ReadFromJsonAsync<AuthenticationResponseDto>();
        Assert.NotNull(token);

        // APIにトークンを付与してリクエスト
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        var response = await client.GetAsync(ApiPath.V1WeatherForecastFull);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<WeatherForecast[]>();

        // レスポンスを確認
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(5, result.Length);
    }
}
