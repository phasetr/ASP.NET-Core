using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Common.Constants;
using Common.Dto;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Test.Integration.WebApi.Controllers.V1;

public class WeatherForecastControllerTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Get_WhenNoAuthCalled_Returns401()
    {
        // HTTPクライアントの初期化
        var client = factory.CreateClient();

        // APIにトークンを付与してリクエスト
        var response = await client.GetAsync(ApiPath.V1WeatherForecastFull);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_WhenAuthCalled_ReturnsOk()
    {
        // HTTPクライアントの初期化
        var client = factory.CreateClient();

        // APIでトークンを取得
        var getTokenDto = new GetTokenDto
        {
            Email = "user@secureapi.com",
            Password = "Pa$$w0rd."
        };
        var json = JsonSerializer.Serialize(getTokenDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var tokenResponse = await client.PostAsync("/api/v1/user/token", content);
        var token = await tokenResponse.Content.ReadFromJsonAsync<AuthenticationResponseDto>();
        Assert.NotNull(token);

        // APIにトークンを付与してリクエスト
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        var response = await client.GetAsync(ApiPath.V1WeatherForecastFull);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<WeatherForecastResponseDto>();
        var weatherForecasts = result?.WeatherForecasts.ToArray();

        // レスポンスを確認
        Assert.NotNull(result);
        Assert.NotNull(weatherForecasts);
        Assert.NotEmpty(weatherForecasts);
        Assert.Equal(5, weatherForecasts.Length);
    }
}
