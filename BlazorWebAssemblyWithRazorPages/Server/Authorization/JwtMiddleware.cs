using BlazorWebAssemblyWithRazorPages.Server.Helpers;
using BlazorWebAssemblyWithRazorPages.Server.Services;
using Microsoft.Extensions.Options;

namespace BlazorWebAssemblyWithRazorPages.Server.Authorization;

public class JwtMiddleware
{
    private readonly AppSettings _appSettings;
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token == null) throw new Exception("No token provided");
        var userId = jwtUtils.ValidateJwtToken(token);

        // attach user to context on successful jwt validation
        if (userId != null) context.Items["User"] = userService.GetById(userId);
        await _next(context);
    }
}
