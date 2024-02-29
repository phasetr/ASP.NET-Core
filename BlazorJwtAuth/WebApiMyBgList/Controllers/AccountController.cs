using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApiMyBgList.Dto;
using WebApiMyBgList.Models;

namespace WebApiMyBgList.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class AccountController(
    ILogger<AccountController> logger,
    IConfiguration configuration,
    UserManager<ApiUser> userManager)
    : ControllerBase
{
    /// <summary>
    ///     Registers a new user.
    /// </summary>
    /// <param name="input">A DTO containing the user data.</param>
    /// <returns>A 201 – Created Status Code in case of success.</returns>
    /// <response code="201">User has been registered</response>
    /// <response code="400">Invalid data</response>
    /// <response code="500">An error occurred</response>
    [HttpPost]
    [ResponseCache(CacheProfileName = "NoCache")]
    [ProducesResponseType(typeof(string), 201)]
    [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    public async Task<ActionResult> Register(RegisterDto input)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var newUser = new ApiUser
                {
                    UserName = input.UserName,
                    Email = input.Email
                };
                var result = await userManager.CreateAsync(
                    newUser, input.Password ?? string.Empty);
                if (!result.Succeeded)
                    throw new Exception(
                        $"Error: {string.Join(" ", result.Errors.Select(e => e.Description))}");
                logger.LogInformation(
                    "User {UserName} ({Email}) has been created",
                    newUser.UserName, newUser.Email);
                return StatusCode(201,
                    $"User '{newUser.UserName}' has been created.");

            }

            var details = new ValidationProblemDetails(ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Status = StatusCodes.Status400BadRequest
            };
            return new BadRequestObjectResult(details);
        }
        catch (Exception e)
        {
            var exceptionDetails = new ProblemDetails
            {
                Detail = e.Message,
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                exceptionDetails);
        }
    }

    /// <summary>
    ///     Performs a user login.
    /// </summary>
    /// <param name="input">A DTO containing the user's credentials.</param>
    /// <returns>The Bearer Token (in JWT format).</returns>
    /// <response code="200">User has been logged in</response>
    /// <response code="400">Login failed (bad request)</response>
    /// <response code="401">Login failed (unauthorized)</response>
    [HttpPost]
    [ResponseCache(CacheProfileName = "NoCache")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 401)]
    public async Task<ActionResult> Login(LoginDto input)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(input.UserName ?? string.Empty);
                if (user == null
                    || !await userManager.CheckPasswordAsync(
                        user, input.Password ?? string.Empty))
                    throw new Exception("Invalid login attempt.");

                var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            configuration["JWT:SigningKey"] ?? string.Empty)),
                    SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new(
                        ClaimTypes.Name, user.UserName ?? string.Empty)
                };
                claims.AddRange(
                    (await userManager.GetRolesAsync(user))
                    .Select(r => new Claim(ClaimTypes.Role, r)));

                var jwtObject = new JwtSecurityToken(
                    configuration["JWT:Issuer"],
                    configuration["JWT:Audience"],
                    claims,
                    expires: DateTime.Now.AddSeconds(300),
                    signingCredentials: signingCredentials);

                var jwtString = new JwtSecurityTokenHandler()
                    .WriteToken(jwtObject);

                return StatusCode(
                    StatusCodes.Status200OK,
                    jwtString);
            }

            var details = new ValidationProblemDetails(ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Status = StatusCodes.Status400BadRequest
            };
            return new BadRequestObjectResult(details);
        }
        catch (Exception e)
        {
            var exceptionDetails = new ProblemDetails
            {
                Detail = e.Message,
                Status = StatusCodes.Status401Unauthorized,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };
            return StatusCode(
                StatusCodes.Status401Unauthorized,
                exceptionDetails);
        }
    }
}
