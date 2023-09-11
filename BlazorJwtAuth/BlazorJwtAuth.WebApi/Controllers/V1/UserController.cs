﻿using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.Common.EntityModels.Entities;
using BlazorJwtAuth.WebApi.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorJwtAuth.WebApi.Controllers.V1;

[Route("api/v1/[controller]")]
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
        _userService = userService;
        _userManager = userManager;
        _claimsService = claimsService;
        _jwtTokenService = jwtTokenService;
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

    [HttpPost("token")]
    public async Task<IActionResult> GetTokenAsync(GetTokenResponseDto model)
    {
        var result = await _userService.GetTokenAsync(model);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenDto model)
    {
        var response = await _userService.RefreshTokenAsync(model.RefreshToken);
        return Ok(response);
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenDto model)
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
    public async Task<ActionResult> RegisterAsync(RegisterDto dto)
    {
        var result = await _userService.RegisterAsync(dto);
        return Ok(result);
    }

    [HttpPost("add-role")]
    public async Task<IActionResult> AddRoleAsync(AddRoleDto dto)
    {
        var result = await _userService.AddRoleAsync(dto);
        return Ok(result);
    }

    [HttpPost("login")]
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

        var token = _jwtTokenService.GetJwtToken(userClaims);

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
    public async Task<IActionResult> GetByEmailAsync(string email)
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
