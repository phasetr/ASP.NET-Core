using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorJwtAuth.Client.Service.Services;
using BlazorJwtAuth.Test.Client.Unit.Helpers;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace BlazorJwtAuth.Test.Client.Unit.Service.Services;

public class SecuredHttpClientServiceTests
{
    [Fact]
    public async Task GetSecuredDataAsync_Unauthorized()
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{Constants.AppSettings.ApiBaseAddress}/Secured")
            .Respond(HttpStatusCode.Unauthorized);
        var mockHttpClient = mockHttp.ToHttpClient();
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory.CreateClient().Returns(mockHttpClient);

        var sut = new SecuredHttpClientService(mockHttpClientFactory);

        var result = await sut.GetSecuredDataAsync(Constants.AppSettings, "token");

        Assert.NotNull(result);
        Assert.Equal("Unauthorized: Please check your token.", result.Message);
        Assert.Equal(HttpStatusCode.Unauthorized.ToString(), result.Status);
    }

    [Fact]
    public async Task GetSecuredDataAsync_Ok()
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{Constants.AppSettings.ApiBaseAddress}/Secured")
            .Respond(HttpStatusCode.OK, "application/json", "Secured data");
        var mockHttpClient = mockHttp.ToHttpClient();
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory.CreateClient().Returns(mockHttpClient);

        var sut = new SecuredHttpClientService(mockHttpClientFactory);

        var result = await sut.GetSecuredDataAsync(Constants.AppSettings, "token");

        Assert.NotNull(result);
        Assert.Equal("Secured data", result.Message);
        Assert.Equal(HttpStatusCode.OK.ToString(), result.Status);
    }

    [Fact]
    public async Task GetSecuredDataAsync_Ambiguous()
    {
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{Constants.AppSettings.ApiBaseAddress}/Secured")
            .Respond(HttpStatusCode.Ambiguous, "application/json", "Ambiguous data");
        var mockHttpClient = mockHttp.ToHttpClient();
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory.CreateClient().Returns(mockHttpClient);

        var sut = new SecuredHttpClientService(mockHttpClientFactory);

        var result = await sut.GetSecuredDataAsync(Constants.AppSettings, "token");

        Assert.NotNull(result);
        Assert.Equal("Oops! Something went wrong.", result.Message);
        Assert.Equal(HttpStatusCode.Ambiguous.ToString(), result.Status);
    }
}
