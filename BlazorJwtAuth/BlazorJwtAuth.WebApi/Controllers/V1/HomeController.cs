using System.Net;
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
            Detail = "",
            Message = "This is get, api/v1",
            Status = HttpStatusCode.OK.ToString()
        });
    }

    [HttpPost]
    public IActionResult PostIndex()
    {
        return Ok(new ResponseBaseDto
        {
            Detail = "",
            Message = "This is post, api/v1",
            Status = HttpStatusCode.OK.ToString()
        });
    }
}
