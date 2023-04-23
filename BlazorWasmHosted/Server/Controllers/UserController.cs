using System.IdentityModel.Tokens.Jwt;
using BlazorWasmHosted.Server.Models;
using BlazorWasmHosted.Server.Services;
using BlazorWasmHosted.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWasmHosted.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IClaimsService _claimsService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(
        UserManager<ApplicationUser> userManager,
        IClaimsService claimsService,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _claimsService = claimsService;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost]
    [Route("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
    {
        ApplicationUser newUser = new()
        {
            Email = userRegisterDto.Email,
            UserName = userRegisterDto.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        if (userRegisterDto.Password == null)
            return CreatedAtAction(nameof(Register), new UserRegisterResultDto {Succeeded = true});

        var result = await _userManager.CreateAsync(newUser, userRegisterDto.Password);

        if (!result.Succeeded)
            return Conflict(new UserRegisterResultDto
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description)
            });
        await _userManager.AddToRoleAsync(newUser, UserRoles.User);
        return CreatedAtAction(nameof(Register), new UserRegisterResultDto {Succeeded = true});
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        if (userLoginDto.Email == null || userLoginDto.Password == null)
            return Unauthorized(new UserLoginResultDto
            {
                Succeeded = false,
                Message = "The email and password combination was invalid."
            });

        var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
            return Unauthorized(new UserLoginResultDto
            {
                Succeeded = false,
                Message = "The email and password combination was invalid."
            });

        var token = _jwtTokenService.GetJwtToken(user);
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
