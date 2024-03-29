﻿using Common.Constants;
using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1;

[Authorize]
[Route(ApiPath.V1Secured)]
[ApiController]
public class SecuredController : ControllerBase
{
    [HttpGet]
    public IActionResult GetSecuredData()
    {
        return Ok(new SecuredDataResponseDto
        {
            Succeeded = true,
            Message = "This Secured Data is available only for Authenticated Users."
        });
    }

    /// <summary>
    ///     admin@secureapi.com/adminpassなど管理権限を持つユーザーに対するJWTを設定すること
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public IActionResult PostSecuredData()
    {
        return Ok(new SecuredDataResponseDto
        {
            Succeeded = true,
            Message = "This Secured Data is available only for Administrators."
        });
    }
}
