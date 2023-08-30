using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.Common.EntityModels.Entities;
using BlazorJwtAuth.Common.Models;
using BlazorJwtAuth.WebApi.Service.Services;
using BlazorJwtAuth.WebApi.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlazorJwtAuth.WebApi.Controllers.V1;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IClaimsService _claimsService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<UserController> _logger;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserService _userService;

    public UserController(
        IClaimsService claimsService,
        IJwtTokenService jwtTokenService,
        ILogger<UserController> logger,
        IUserService userService,
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userService = userService;
        _userManager = userManager;
        _claimsService = claimsService;
        _jwtTokenService = jwtTokenService;
        _roleManager = roleManager;
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetTokenAsync(GetTokenRequest model)
    {
        var result = await _userService.GetTokenAsync(model);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest model)
    {
        var response = await _userService.RefreshTokenAsync(model.RefreshToken);
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

    [Authorize]
    [HttpPost("tokens/{id}")]
    public async Task<IActionResult> GetRefreshTokensAsync(string id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user is null) return NotFound();
        return Ok(user.RefreshTokens);
    }

    [HttpPost("register")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult> RegisterAsync(RegisterModel model)
    {
        var result = await _userService.RegisterAsync(model);
        return Ok(result);
    }

    [HttpPost("add-role")]
    public async Task<IActionResult> AddRoleAsync(AddRoleModel model)
    {
        var result = await _userService.AddRoleAsync(model);
        return Ok(result);
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(UserLoginDto userLoginDto)
    {
        var user = await _userService.GetByEmailAsync(userLoginDto.Email ?? string.Empty);
        if (user == null || !await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
            return Unauthorized(new UserLoginResultDto
            {
                Succeeded = false,
                Message = "The email and password combination was invalid."
            });
        var userClaims = await _claimsService.GetUserClaimsAsync(user);

        var token = _jwtTokenService.GetJwtToken(userClaims);

        return Ok(new UserLoginResultDto
        {
            Succeeded = true,
            Token = new TokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            }
        });
    }
}
