using System.Net;
using BlazorJwtAuth.Common.Models;
using BlazorJwtAuth.WebApi.Controllers.V1;
using Microsoft.AspNetCore.Mvc;

namespace BlazorJwtAuth.Test.Unit.WebApi.Controllers.V1;

public class HomeControllerTests
{
    [Fact]
    public void GetIndex_Should_Return_Ok()
    {
        var controller = new HomeController();
        var response = controller.GetIndex();
        var responseObject = Assert.IsType<OkObjectResult>(response);
        var result = Assert.IsAssignableFrom<ResponseBase>(responseObject.Value);
        Assert.Empty(result.Detail);
        Assert.Equal("This is get, api/v1", result.Message);
        Assert.Equal(HttpStatusCode.OK.ToString(), result.Status);
    }

    [Fact]
    public void PostIndex_Should_Return_Ok()
    {
        var controller = new HomeController();
        var response = controller.PostIndex();
        var responseObject = Assert.IsType<OkObjectResult>(response);
        var result = Assert.IsAssignableFrom<ResponseBase>(responseObject.Value);
        Assert.Empty(result.Detail);
        Assert.Equal("This is post, api/v1", result.Message);
        Assert.Equal(HttpStatusCode.OK.ToString(), result.Status);
    }
}
