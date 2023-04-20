using System.IdentityModel.Tokens.Jwt;
using BlazorWasmHosted.Server.DTOs;
using BlazorWasmHosted.Server.Models;
using BlazorWasmHosted.Server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWasmHosted.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IClaimsService _claimsService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly RoleManager<ApplicationRole> _roleManager;

    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IClaimsService claimsService,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
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

        await SeedRoles();
        await _userManager.AddToRoleAsync(newUser, UserRoles.User);

        return CreatedAtAction(nameof(Register), new UserRegisterResultDto {Succeeded = true});
    }

    private async Task SeedRoles()
    {
        if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            await _roleManager.CreateAsync(new ApplicationRole {Name = UserRoles.Admin});

        if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            await _roleManager.CreateAsync(new ApplicationRole {Name = UserRoles.User});
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
