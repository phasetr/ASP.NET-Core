using System.Net;
using System.Net.Http.Json;
using Common.Constants;
using Common.Dto;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Test.Integration.WebApi.Controllers.V1;

public class UserControllerTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Get_UrlChecker()
    {
        // HTTPクライアントの初期化
        var client = factory.CreateClient();

        // APIで結果を取得
        var response = await client.GetAsync(ApiPath.V1User);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ResponseBaseDto>();

        // レスポンスを確認
        Assert.NotNull(result);
        Assert.Equal("This is the user controller!", result.Message);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ExistingUser()
    {
        // HTTPクライアントの初期化
        var client = factory.CreateClient();

        // APIで結果を取得
        var response = await client.GetAsync(ApiPath.V1User + "/user@secureapi.com");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<UserGetByEmailResponseDto>();

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
        var client = factory.CreateClient();

        // APIで結果を取得
        var response = await client.GetAsync("/api/v1/User/nouser@secureapi.com");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
