using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorJwtAuth.Client.Service.Services;
using BlazorJwtAuth.Common.Constants;
using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.Test.Client.Unit.Helpers;
using RichardSzalay.MockHttp;

namespace BlazorJwtAuth.Test.Client.Unit.Service.Services;

public class SecuredHttpClientServiceTests
{
    [Fact]
    public async Task GetSecuredDataAsync_Unauthorized()
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{Constants.AppSettings.ApiBaseAddress}/{ApiPath.V1Secured}")
            .Respond(
                HttpStatusCode.Unauthorized,
                "application/json",
                JsonSerializer.Serialize(new SecuredDataResponseDto
                {
                    Message = "Unauthorized: Please check your token.",
                    Status = HttpStatusCode.Unauthorized.ToString()
                }));
        var mockHttpClient = mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = new Uri(Constants.AppSettings.ApiBaseAddress);

        var sut = new SecuredHttpClientService();
        var result = await sut.GetSecuredDataAsync(mockHttpClient, "token");

        Assert.NotNull(result);
        Assert.Equal("Unauthorized: Please check your token.", result.Message);
        Assert.Equal(HttpStatusCode.Unauthorized.ToString(), result.Status);
    }

    [Fact]
    public async Task GetSecuredDataAsync_Ok()
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{Constants.AppSettings.ApiBaseAddress}/{ApiPath.V1Secured}")
            .Respond(
                HttpStatusCode.OK,
                "application/json",
                JsonSerializer.Serialize(new SecuredDataResponseDto
                {
                    Message = "message",
                    Status = HttpStatusCode.OK.ToString()
                }));
        var mockHttpClient = mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = new Uri(Constants.AppSettings.ApiBaseAddress);

        var sut = new SecuredHttpClientService();
        var result = await sut.GetSecuredDataAsync(mockHttpClient, "token");

        Assert.NotNull(result);
        Assert.Equal("message", result.Message);
        Assert.Equal(HttpStatusCode.OK.ToString(), result.Status);
    }

    [Fact]
    public async Task GetSecuredDataAsync_Ambiguous()
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{Constants.AppSettings.ApiBaseAddress}/{ApiPath.V1Secured}")
            .Respond(
                HttpStatusCode.Ambiguous,
                "application/json",
                JsonSerializer.Serialize(new SecuredDataResponseDto
                {
                    Message = "\"Ambiguous data\"",
                    Status = HttpStatusCode.Ambiguous.ToString()
                }));
        var mockHttpClient = mockHttp.ToHttpClient();
        mockHttpClient.BaseAddress = new Uri(Constants.AppSettings.ApiBaseAddress);

        var sut = new SecuredHttpClientService();
        var result = await sut.GetSecuredDataAsync(mockHttpClient, "token");

        Assert.NotNull(result);
        Assert.Equal("Oops! Something went wrong.", result.Message);
        Assert.Equal(HttpStatusCode.Ambiguous.ToString(), result.Status);
    }
}
