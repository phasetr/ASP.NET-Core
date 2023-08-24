using System;
using System.Threading.Tasks;
using BlazorJwtAuth.Common.Models;
using BlazorJwtAuth.WebApi.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorJwtAuth.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult> RegisterAsync(RegisterModel model)
    {
        var result = await _userService.RegisterAsync(model);
        return Ok(result);
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetTokenAsync(TokenRequestModel model)
    {
        var result = await _userService.GetTokenAsync(model);
        if (!string.IsNullOrEmpty(result.RefreshToken)) SetRefreshTokenInCookie(result.RefreshToken);
        return Ok(result);
    }

    [HttpPost("add-role")]
    public async Task<IActionResult> AddRoleAsync(AddRoleModel model)
    {
        var result = await _userService.AddRoleAsync(model);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var response = await _userService.RefreshTokenAsync(refreshToken ?? string.Empty);
        if (!string.IsNullOrEmpty(response.RefreshToken))
            SetRefreshTokenInCookie(response.RefreshToken);
        return Ok(response);
    }


    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
    {
        // accept token from request body or cookie
        // var token = model.Token ?? Request.Cookies["refreshToken"];
        var token = model.Token;

        if (string.IsNullOrEmpty(token))
            return BadRequest(new {message = "Token is required"});

        var response = await _userService.RevokeTokenAsync(token);

        if (!response) return NotFound(new {message = "Token not found"});

        return Ok(new {message = "Token revoked"});
    }

    private void SetRefreshTokenInCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(10)
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }

    [Authorize]
    [HttpPost("tokens/{id}")]
    public async Task<IActionResult> GetRefreshTokensAsync(string id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user is null) return NotFound();
        return Ok(user.RefreshTokens);
    }
}
