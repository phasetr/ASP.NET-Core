using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IdentityByRazorPages.Controllers;

[ApiController]
[Route("/api/account")]
public class ApiAccountController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly SignInManager<IdentityUser> _signinManager;
    private readonly UserManager<IdentityUser> _userManager;

    public ApiAccountController(SignInManager<IdentityUser> mgr,
        UserManager<IdentityUser> userMgr, IConfiguration config)
    {
        _signinManager = mgr;
        _userManager = userMgr;
        _configuration = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Credentials credentials)
    {
        var result
            = await _signinManager.PasswordSignInAsync(credentials.Username,
                credentials.Password, false, false);
        if (result.Succeeded) return Ok();
        return Unauthorized();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signinManager.SignOutAsync();
        return Ok();
    }

    [HttpPost("token")]
    public async Task<IActionResult> Token([FromBody] Credentials credentials)
    {
        if (!await CheckPassword(credentials)) return Unauthorized();
        var handler = new JwtSecurityTokenHandler();
        var secret = Encoding.ASCII.GetBytes(_configuration["jwtSecret"]);
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, credentials.Username)
            }),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(secret),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = handler.CreateToken(descriptor);
        return Ok(new
        {
            success = true,
            token = handler.WriteToken(token)
        });
    }

    private async Task<bool> CheckPassword(Credentials credentials)
    {
        var user = await _userManager.FindByNameAsync(credentials.Username);
        if (user != null)
            return (await _signinManager.CheckPasswordSignInAsync(user,
                credentials.Password, true)).Succeeded;
        return false;
    }

    public class Credentials
    {
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}