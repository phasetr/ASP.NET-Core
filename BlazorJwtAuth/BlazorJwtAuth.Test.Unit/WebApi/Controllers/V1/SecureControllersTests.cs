using BlazorJwtAuth.WebApi.Controllers.V1;
using Microsoft.AspNetCore.Mvc;

namespace BlazorJwtAuth.Test.Unit.WebApi.Controllers.V1;

public class SecureControllersTests
{
    [Fact]
    public void GetSecureData_Should_Return_Ok()
    {
        var controller = new SecuredController();
        var result = controller.GetSecuredData();
        var resultObject = Assert.IsType<OkObjectResult>(result);
        var okValue = Assert.IsAssignableFrom<string>(resultObject.Value);
        Assert.Equal("This Secured Data is available only for Authenticated Users.",
            okValue);
    }
    
    [Fact]
    public void PostSecureData_Should_Return_Ok()
    {
        var controller = new SecuredController();
        var result = controller.PostSecuredData();
        var resultObject = Assert.IsType<OkObjectResult>(result);
        var okValue = Assert.IsAssignableFrom<string>(resultObject.Value);
        Assert.Equal("This Secured Data is available only for Administrators.",
            okValue);
    }
}
