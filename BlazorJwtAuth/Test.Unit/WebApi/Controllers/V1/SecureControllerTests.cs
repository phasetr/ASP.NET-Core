using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.V1;

namespace Test.Unit.WebApi.Controllers.V1;

public class SecureControllerTests
{
    [Fact]
    public void GetSecureData_Should_Return_Ok()
    {
        var controller = new SecuredController();
        var response = controller.GetSecuredData();
        var responseObject = Assert.IsType<OkObjectResult>(response);
        var result = Assert.IsAssignableFrom<SecuredDataResponseDto>(responseObject.Value);
        Assert.Equal("This Secured Data is available only for Authenticated Users.", result.Message);
    }

    [Fact]
    public void PostSecureData_Should_Return_Ok()
    {
        var controller = new SecuredController();
        var response = controller.PostSecuredData();
        var responseObject = Assert.IsType<OkObjectResult>(response);
        var result = Assert.IsAssignableFrom<SecuredDataResponseDto>(responseObject.Value);
        Assert.Equal("This Secured Data is available only for Administrators.", result.Message);
    }
}
