using System;
using System.Text.Json;
using System.Threading.Tasks;
using Client.Services;
using Client.Services.Interfaces;
using Common.Constants;
using Common.Dto;
using Common.Models;
using Common.Services;
using NSubstitute;
using RichardSzalay.MockHttp;
using Test.Client.Unit.Helpers;

namespace Test.Client.Unit.Service.Services;

public class WeatherForecastHttpClientServiceTests
{
    private readonly PtDateTime _ptDateTime = new();
    private WeatherForecastHttpClientService _sut = default!;

    [Fact]
    public async Task GetForecastAsync_EmptyResponse()
    {
        var now = _ptDateTime.UtcNow;
        var weatherForecasts = Array.Empty<WeatherForecastResponseDto>();
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{Constants.AppSettings.ApiBaseAddress}/{ApiPath.V1WeatherForecastFull}")
            .Respond("application/json", JsonSerializer.Serialize(weatherForecasts));
        var mockHttpClient = mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = new Uri(Constants.AppSettings.ApiBaseAddress);

        var mockTokenService = Substitute.For<ITokenService>();
        mockTokenService.GetTokenAsync().Returns(new TokenDto
        {
            Token = "token",
            Expiration = now.AddHours(1)
        });

        _sut = new WeatherForecastHttpClientService(_ptDateTime, mockTokenService);
        var result = await _sut.GetForecastAsync(mockHttpClient);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetForecastAsync_NonEmptyResponse()
    {
        var now = _ptDateTime.UtcNow;
        var weatherForecasts = new[]
        {
            new WeatherForecast
            {
                Date = now,
                Summary = "Summary",
                TemperatureC = 0
            }
        };
        var weatherForecastResponseDto = new WeatherForecastResponseDto
        {
            WeatherForecasts = weatherForecasts
        };
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{Constants.AppSettings.ApiBaseAddress}/{ApiPath.V1WeatherForecastFull}")
            .Respond("application/json", JsonSerializer.Serialize(weatherForecastResponseDto));
        var mockHttpClient = mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = new Uri(Constants.AppSettings.ApiBaseAddress);

        var mockTokenService = Substitute.For<ITokenService>();
        mockTokenService.GetTokenAsync().Returns(new TokenDto
        {
            Token = "token",
            Expiration = now.AddHours(1)
        });

        _sut = new WeatherForecastHttpClientService(_ptDateTime, mockTokenService);
        var result = await _sut.GetForecastAsync(mockHttpClient);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(weatherForecasts[0].Date, result[0].Date);
        Assert.Equal(weatherForecasts[0].Summary, result[0].Summary);
        Assert.Equal(weatherForecasts[0].TemperatureC, result[0].TemperatureC);
    }
}
