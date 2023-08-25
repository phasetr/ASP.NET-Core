using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorJwtAuth.WebApi.Controllers.V1;

[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class SecuredController : ControllerBase
{
    [HttpGet]
    public IActionResult GetSecuredData()
    {
        return Ok("This Secured Data is available only for Authenticated Users.");
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public IActionResult PostSecuredData()
    {
        return Ok("This Secured Data is available only for Administrators.");
    }
}
