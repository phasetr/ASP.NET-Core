using System.Net;
using BlazorJwtAuth.Common.Dto;
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
        return Ok(new SecuredDataResultDto
        {
            Message = "This Secured Data is available only for Authenticated Users.",
            Status = HttpStatusCode.OK.ToString()
        });
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public IActionResult PostSecuredData()
    {
        return Ok(new SecuredDataResultDto
        {
            Message = "This Secured Data is available only for Administrators.",
            Status = HttpStatusCode.OK.ToString()
        });
    }
}
