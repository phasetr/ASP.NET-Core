using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorJwtAuth.Client.Common.Library;
using BlazorJwtAuth.Client.Service.Services;
using BlazorJwtAuth.Client.Service.Services.Interfaces;
using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.Common.Services;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace BlazorJwtAuth.Test.Client.Unit.Service.Services;

public class WeatherForecastHttpClientServiceTests
{
    private readonly AppSettings _appSettings = new()
    {
        ApiBaseAddress = "https://localhost:5001"
    };

    private readonly PtDateTime _ptDateTime = new();
    private WeatherForecastHttpClientService _sut = default!;

    [Fact]
    public async Task GetForecastAsync_EmptyResponse()
    {
        var now = _ptDateTime.UtcNow;
        var weatherForecasts = System.Array.Empty<WeatherForecastDto>();
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{_appSettings.ApiBaseAddress}/WeatherForecast")
            .Respond("application/json", JsonSerializer.Serialize(weatherForecasts));
        var mockHttpClient = mockHttp.ToHttpClient();
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory.CreateClient().Returns(mockHttpClient);

        var mockTokenService = Substitute.For<ITokenService>();
        mockTokenService.GetTokenAsync().Returns(new TokenDto
        {
            Token = "token",
            Expiration = now.AddHours(1)
        });

        _sut = new WeatherForecastHttpClientService(
            mockHttpClientFactory,
            _ptDateTime,
            mockTokenService);

        var result = await _sut.GetForecastAsync(_appSettings);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetForecastAsync_NonEmptyResponse()
    {
        var now = _ptDateTime.UtcNow;
        var weatherForecasts = new[]
        {
            new WeatherForecastDto
            {
                Date = now,
                Summary = "Summary",
                TemperatureC = 0
            }
        };
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{_appSettings.ApiBaseAddress}/WeatherForecast")
            .Respond("application/json", JsonSerializer.Serialize(weatherForecasts));
        var mockHttpClient = mockHttp.ToHttpClient();
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory.CreateClient().Returns(mockHttpClient);

        var mockTokenService = Substitute.For<ITokenService>();
        mockTokenService.GetTokenAsync().Returns(new TokenDto
        {
            Token = "token",
            Expiration = now.AddHours(1)
        });

        _sut = new WeatherForecastHttpClientService(
            mockHttpClientFactory,
            _ptDateTime,
            mockTokenService);

        var result = await _sut.GetForecastAsync(_appSettings);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(weatherForecasts[0].Date, result[0].Date);
        Assert.Equal(weatherForecasts[0].Summary, result[0].Summary);
        Assert.Equal(weatherForecasts[0].TemperatureC, result[0].TemperatureC);
    }
}
