using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Authentication;
using WebApi.Services;
using WebApi.Services.Authorization;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IApplicationUserService _applicationUserService;

    public UsersController(IApplicationUserService applicationUserService)
    {
        _applicationUserService = applicationUserService;
    }

    [MyAllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<IActionResult> AuthenticateAsync(Request model)
    {
        var response = await _applicationUserService.AuthenticateAsync(model, IpAddress());
        SetTokenCookie(response.RefreshToken);
        return Ok(response);
    }

    [MyAllowAnonymous]
    [HttpPost("refresh-token")]
    public IActionResult RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var response = _applicationUserService.RefreshToken(refreshToken, IpAddress());
        SetTokenCookie(response.RefreshToken);
        return Ok(response);
    }

    [HttpPost("revoke-token")]
    public IActionResult RevokeToken(RevokeTokenRequest model)
    {
        // accept refresh token in request body or cookie
        var token = model.Token ?? Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(token))
            return BadRequest(new {message = "Token is required"});

        _applicationUserService.RevokeToken(token, IpAddress());
        return Ok(new {message = "Token revoked"});
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _applicationUserService.GetAll();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        var user = _applicationUserService.GetById(id);
        return Ok(user);
    }

    [HttpGet("{id}/refresh-tokens")]
    public IActionResult GetRefreshTokens(string id)
    {
        var user = _applicationUserService.GetById(id);
        return Ok(user.RefreshTokens);
    }

    // helper methods

    private void SetTokenCookie(string token)
    {
        // append cookie with refresh token to the http response
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }

    private string IpAddress()
    {
        // get source ip address for the current request
        if (Request.Headers.TryGetValue("X-Forwarded-For", out var address))
            return address;
        return HttpContext.Connection.RemoteIpAddress != null
            ? HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()
            : "null";
    }
}
