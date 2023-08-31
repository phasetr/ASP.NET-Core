using Microsoft.AspNetCore.Mvc;

namespace BlazorJwtAuth.WebApi.Controllers.V1;

[Route("api/v1/")]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult GetIndex()
    {
        return Ok("This is get, api/v1");
    }
    
    [HttpPost]
    public IActionResult PostIndex()
    {
        return Ok("This is post, api/v1");
    }
}
