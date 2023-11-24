using Common.Constants;
using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route(ApiPath.AuthRoot)]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var envRegion = Environment.GetEnvironmentVariable("REGION");
        var envCognitoUserPoolId = Environment.GetEnvironmentVariable("COGNITO_USER_POOL_ID");
        var envClientId = Environment.GetEnvironmentVariable("CLIENT_ID");
        Console.WriteLine($"REGION: {envRegion}");
        Console.WriteLine($"COGNITO_USER_POOL_ID: {envCognitoUserPoolId}");
        Console.WriteLine($"CLIENT_ID: {envClientId}");
        if (string.IsNullOrEmpty(envRegion) ||
            string.IsNullOrEmpty(envCognitoUserPoolId) ||
            string.IsNullOrEmpty(envClientId))
            throw new ArgumentNullException("REGION or COGNITO_USER_POOL_ID or CLIENT_ID are null");

        return Ok(new ResponseBaseDto
        {
            Message = "Authentication Succeeded",
            Succeeded = true
        });
    }
}