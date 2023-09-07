using System.Net;
using System.Net.Http.Json;
using BlazorJwtAuth.Common.Dto;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BlazorJwtAuth.Test.Integration.WebApi.Controllers.V1;

public class UserControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public UserControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_UrlChecker()
    {
        // HTTPクライアントの初期化
        var client = _factory.CreateClient();

        // APIで結果を取得
        var response = await client.GetAsync("/api/v1/User");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();

        // レスポンスを確認
        Assert.NotEmpty(result);
        Assert.Equal("This is the user controller!", result);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ExistingUser()
    {
        // HTTPクライアントの初期化
        var client = _factory.CreateClient();

        // APIで結果を取得
        var response = await client.GetAsync($"/api/v1/User/user@secureapi.com");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<UserGetByEmailResultDto>();

        // レスポンスを確認
        Assert.NotNull(result);
        Assert.Equal("userId", result.UserId);
        Assert.Equal("user", result.UserName);
        Assert.Equal("First", result.FirstName);
        Assert.Equal("Last", result.LastName);
    }

    [Fact]
    public async Task GetUserByEmailAsync_NonExistentUser()
    {
        // HTTPクライアントの初期化
        var client = _factory.CreateClient();

        // APIで結果を取得
        var response = await client.GetAsync($"/api/v1/User/nouser@secureapi.com");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
