using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BlazorJwtAuth.Common.DataContext.Data;
using BlazorJwtAuth.Common.Models;
using BlazorJwtAuth.Test.Integration.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

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
        // データベースの初期化
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();
            Utilities.ReinitializeDbForTests(db);
        }

        // HTTPクライアントの初期化
        var client = _factory.CreateClient();

        // APIでトークンを取得
        var getTokenRequest = new GetTokenRequest
        {
            Email = "user@secureapi.com",
            Password = "Pa$$w0rd."
        };
        var json = JsonSerializer.Serialize(getTokenRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var tokenResponse = await client.PostAsync("/api/v1/User/token", content);
        var token = await tokenResponse.Content.ReadFromJsonAsync<AuthenticationResponse>();
        Assert.NotNull(token);

        // APIにトークンを付与してリクエスト
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        var response = await client.GetAsync("api/v1/Secured");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();

        // レスポンスを確認
        Assert.NotEmpty(result);
        Assert.Equal("This Secured Data is available only for Authenticated Users.", result);
    }

    [Fact]
    public async Task PostSecuredData_WhenCalled_ReturnsOk()
    {
        // データベースの初期化
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();
            Utilities.ReinitializeDbForTests(db);
        }

        // HTTPクライアントの初期化
        var client = _factory.CreateClient();

        // APIでトークンを取得
        var getTokenRequest = new GetTokenRequest
        {
            Email = "admin@secureapi.com",
            Password = "adminpass"
        };
        var json = JsonSerializer.Serialize(getTokenRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var tokenResponse = await client.PostAsync("/api/v1/User/token", content);
        var token = await tokenResponse.Content.ReadFromJsonAsync<AuthenticationResponse>();
        Assert.NotNull(token);

        // APIにトークンを付与してPOSTリクエスト
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        var response = await client.PostAsync("api/v1/Secured", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();

        // レスポンスを確認
        Assert.NotEmpty(result);
        Assert.Equal("This Secured Data is available only for Administrators.", result);
    }
}
