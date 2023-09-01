using Microsoft.AspNetCore.Mvc.Testing;

namespace BlazorJwtAuth.Test.Integration.WebApi.Controllers.V1;

public class HomeControllerTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HomeControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/api/v1")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("text/plain; charset=utf-8",
            response.Content.Headers.ContentType?.ToString());
    }

    [Fact]
    public async Task Post_EndpointsReturnSuccessAndCorrectContentType()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsync("/api/v1", null);
        response.EnsureSuccessStatusCode();
        Assert.Equal("text/plain; charset=utf-8",
            response.Content.Headers.ContentType?.ToString());
    }
}
