using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Common.Dto;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ServerlessApi.Service.Interfaces;
using ServerlessApi.Tests.Unit.Mocks;

namespace ServerlessApi.Tests.Unit.Controllers;

public class BookControllerTest
{
    private readonly WebApplicationFactory<Program> _webApplication;

    public BookControllerTest()
    {
        _webApplication = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Mock the repository implementation
                    // to remove infra dependencies for Test project
                    services.AddScoped<IBookService, MockBookService>();
                });
            });
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    public async Task Call_GetApiBooks_ShouldReturn_LimitedListOfBooks(int limit)
    {
        var client = _webApplication.CreateClient();
        var response = await client.GetFromJsonAsync<BookGetResponseDto>($"/api/Books?limit={limit}");

        Assert.NotNull(response);
        Assert.NotNull(response.Books);
        Assert.NotEmpty(response.Books);
        Assert.Equal(limit, response.Books.Count());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public async Task Call_GetApiBook_ShouldReturn_BadRequest(int limit)
    {
        var client = _webApplication.CreateClient();
        var result = await client.GetAsync($"/api/Books?limit={limit}");

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

        var response = await result.Content.ReadFromJsonAsync<BookGetResponseDto>();
        Assert.NotNull(response);
        Assert.Equal("The limit should been between [1-100]", response.Message);
        Assert.False(response.IsSucceed);
    }
}
