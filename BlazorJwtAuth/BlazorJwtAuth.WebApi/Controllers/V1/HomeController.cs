using BlazorJwtAuth.Common.Constants;
using BlazorJwtAuth.Common.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BlazorJwtAuth.WebApi.Controllers.V1;

[Route(ApiPath.V1Home)]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult GetIndex()
    {
        return Ok(new ResponseBaseDto
        {
            Message = "This is get, api/v1"
        });
    }

    [HttpPost]
    public IActionResult PostIndex()
    {
        return Ok(new ResponseBaseDto
        {
            Message = "This is post, api/v1"
        });
    }
}
