using System.Net;
using BlazorJwtAuth.Client.Pages;
using BlazorJwtAuth.Client.Service.Services;
using BlazorJwtAuth.Client.Service.Services.Interfaces;
using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.Test.Client.Unit.Helpers;
using RichardSzalay.MockHttp;

namespace BlazorJwtAuth.Test.Client.Unit.Pages;

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
        mockHttpClient.When("/")
            .RespondJson(new ResponseBaseDto
            {
                Detail = "",
                Message = "message",
                Status = HttpStatusCode.OK.ToString()
            });
        Services.AddScoped<IHomeHttpClientService, HomeHttpClientService>();

        var cut = RenderComponent<Index>();
        cut.Find("#apiBaseAddress")
            .MarkupMatches($"""<dd id="apiBaseAddress">{Constants.AppSettings.ApiBaseAddress}</dd>""");
        cut.WaitForElements("#getMessage")
            .MarkupMatches("""<dd id="getMessage">message</dd>""");
    }
}
