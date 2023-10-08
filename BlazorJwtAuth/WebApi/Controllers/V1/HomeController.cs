using Common.Constants;
using Common.Dto;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1;

[Route(ApiPath.V1Home)]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult GetIndex()
    {
        return Ok(new ResponseBaseDto
        {
            Succeeded = true,
            Message = "This is get, api/v1"
        });
    }

    [HttpPost]
    public IActionResult PostIndex()
    {
        return Ok(new ResponseBaseDto
        {
            Succeeded = true,
            Message = "This is post, api/v1"
        });
    }
}
