using WebApi.Models;

namespace WebApi.Services.Authorization;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    ///     認証済みユーザーの情報をHttpContextに追加する。
    /// </summary>
    /// <param name="context"></param>
    /// <param name="applicationUserService"></param>
    /// <param name="jwtUtils"></param>
    public async Task Invoke(HttpContext context, IApplicationUserService applicationUserService, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.ValidateJwtToken(token);
        // attach user to context on successful jwt validation
        if (userId != null) context.Items[nameof(ApplicationUser)] = await applicationUserService.GetByIdAsync(userId);
        await _next(context);
    }
}
