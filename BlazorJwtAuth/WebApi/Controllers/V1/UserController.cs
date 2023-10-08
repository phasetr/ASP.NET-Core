using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Common.Constants;
using Common.Dto;
using Common.EntityModels.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Service.Services.Interfaces;

namespace WebApi.Controllers.V1;

[Route(ApiPath.V1User)]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IClaimsService _claimsService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserService _userService;

    public UserController(
        IClaimsService claimsService,
        IJwtTokenService jwtTokenService,
        IUserService userService,
        UserManager<ApplicationUser> userManager)
    {
        _claimsService = claimsService;
        _jwtTokenService = jwtTokenService;
        _userService = userService;
        _userManager = userManager;
    }

    /// <summary>
    ///     単なるURLのテスト用
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetAsync()
    {
        return Ok("This is the user controller!");
    }

    [HttpPost(ApiPath.V1UserGetToken)]
    public async Task<IActionResult> GetTokenAsync(GetTokenResponseDto dto)
    {
        var result = await _userService.GetTokenAsync(dto);
        return Ok(result);
    }

    [HttpPost(ApiPath.V1UserRefreshToken)]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenDto model)
    {
        var response = await _userService.RefreshTokenAsync(model.RefreshToken);
        return Ok(response);
    }

    [HttpPost(ApiPath.V1UserRevokeToken)]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenDto dto)
    {
        var token = dto.Token;

        if (string.IsNullOrEmpty(token))
            return BadRequest(new {message = "Token is required"});

        var response = await _userService.RevokeTokenAsync(token);

        if (!response) return NotFound(new {message = "Token not found"});

        return Ok(new {message = "Token revoked"});
    }

    [Authorize]
    [HttpPost(ApiPath.V1UserRefreshTokens + "/{id}")]
    public async Task<IActionResult> GetRefreshTokensAsync(string id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user is null) return NotFound();
        return Ok(user.RefreshTokens);
    }

    [HttpPost(ApiPath.V1UserRegister)]
    public async Task<ActionResult> RegisterAsync(UserRegisterDto dto)
    {
        var result = await _userService.RegisterAsync(dto);
        return Ok(result);
    }

    [HttpPost(ApiPath.V1UserAddRole)]
    public async Task<IActionResult> AddRoleAsync(AddRoleDto dto)
    {
        var result = await _userService.AddRoleAsync(dto);
        return Ok(result);
    }

    [HttpPost(ApiPath.V1UserLogin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(UserLoginDto userLoginDto)
    {
        var user = await _userService.GetByEmailAsync(userLoginDto.Email ?? string.Empty);
        if (user == null || !await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
            return Unauthorized(new UserLoginResponseDto
            {
                Succeeded = false,
                Message = "The email and password combination was invalid."
            });
        var userClaims = await _claimsService.GetUserClaimsAsync(user);
        var identity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        // Generate JWT token
        var token = _jwtTokenService.GetJwtToken(userClaims);
        // クッキーを設定
        Response.Cookies.Append(
            Authorization.JwtAccessTokenName,
            new JwtSecurityTokenHandler().WriteToken(token),
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddMinutes(10)
            });

        // TODO：クッキーへの移行
        return Ok(new UserLoginResponseDto
        {
            Succeeded = true,
            Token = new TokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            }
        });
    }

    [HttpGet("{email}")]
    public async Task<IActionResult> GetByEmailAsync(string email = Authorization.DefaultEmail)
    {
        var user = await _userService.GetByEmailAsync(email);
        if (user is null) return NotFound();
        return Ok(new UserGetByEmailResponseDto
        {
            UserId = user.Id,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName
        });
    }
}
