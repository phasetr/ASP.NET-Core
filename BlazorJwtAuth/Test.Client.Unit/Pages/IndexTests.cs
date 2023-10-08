using Client.Pages;
using Client.Services;
using Client.Services.Interfaces;
using Common.Constants;
using Common.Dto;
using RichardSzalay.MockHttp;
using Test.Client.Unit.Helpers;

namespace Test.Client.Unit.Pages;

public class IndexTests : TestContext
{
    [Fact]
    public void H1_StringContent()
    {
        Services.AddSingleton(Constants.AppSettings);
        Services.AddScoped<IHomeHttpClientService, HomeHttpClientService>();
        var cut = RenderComponent<Index>();
        cut.Find("h1").MarkupMatches("<h1>Hello, world!</h1>");
    }

    [Fact]
    public void ApiCall_Get_Test()
    {
        Services.AddSingleton(Constants.AppSettings);
        var mockHttpClient = Services.AddMockHttpClient();
        mockHttpClient.When($"{Constants.AppSettings.ApiBaseAddress}/{ApiPath.V1Home}")
            .RespondJson(new ResponseBaseDto
            {
                Message = "message"
            });
        Services.AddScoped<IHomeHttpClientService, HomeHttpClientService>();

        var cut = RenderComponent<Index>();
        cut.Find("#apiBaseAddress")
            .MarkupMatches($"""<dd id="apiBaseAddress">{Constants.AppSettings.ApiBaseAddress}</dd>""");
        cut.WaitForElements("#getMessage")
            .MarkupMatches("""<dd id="getMessage">message</dd>""");
    }
}
