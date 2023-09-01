using BlazorJwtAuth.WebApi.Controllers.V1;
using Microsoft.AspNetCore.Mvc;

namespace BlazorJwtAuth.Test.Unit.WebApi.Controllers.V1;

public class HomeControllerTests
{
    [Fact]
    public void GetIndex_Should_Return_Ok()
    {
        var controller = new HomeController();
        var result = controller.GetIndex();
        var resultObject = Assert.IsType<OkObjectResult>(result);
        var okValue = Assert.IsAssignableFrom<string>(resultObject.Value);
        Assert.Equal("This is get, api/v1", okValue);
    }

    [Fact]
    public void PostIndex_Should_Return_Ok()
    {
        var controller = new HomeController();
        var result = controller.PostIndex();
        var resultObject = Assert.IsType<OkObjectResult>(result);
        var okValue = Assert.IsAssignableFrom<string>(resultObject.Value);
        Assert.Equal("This is post, api/v1", okValue);
    }
}
