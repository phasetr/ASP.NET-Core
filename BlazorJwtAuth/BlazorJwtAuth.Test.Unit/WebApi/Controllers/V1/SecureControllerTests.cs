using System.Net;
using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.WebApi.Controllers.V1;
using Microsoft.AspNetCore.Mvc;

namespace BlazorJwtAuth.Test.Unit.WebApi.Controllers.V1;

public class SecureControllerTests
{
    [Fact]
    public void GetSecureData_Should_Return_Ok()
    {
        var controller = new SecuredController();
        var response = controller.GetSecuredData();
        var responseObject = Assert.IsType<OkObjectResult>(response);
        var result = Assert.IsAssignableFrom<SecuredDataResponseDto>(responseObject.Value);
        Assert.Null(result.Detail);
        Assert.Equal("This Secured Data is available only for Authenticated Users.", result.Message);
        Assert.Equal(HttpStatusCode.OK.ToString(), result.Status);
    }

    [Fact]
    public void PostSecureData_Should_Return_Ok()
    {
        var controller = new SecuredController();
        var response = controller.PostSecuredData();
        var responseObject = Assert.IsType<OkObjectResult>(response);
        var result = Assert.IsAssignableFrom<SecuredDataResponseDto>(responseObject.Value);
        Assert.Null(result.Detail);
        Assert.Equal("This Secured Data is available only for Administrators.", result.Message);
        Assert.Equal(HttpStatusCode.OK.ToString(), result.Status);
    }
}
