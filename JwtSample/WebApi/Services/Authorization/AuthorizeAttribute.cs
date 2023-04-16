using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Models;

namespace WebApi.Services.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    /// <summary>
    ///     コントローラーにつけると、そのコントローラーの全てのアクションに対して認証が必要になる。
    ///     特にJWT認証を有効化する。
    /// </summary>
    /// <param name="context"></param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // skip authorization if action is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<MyAllowAnonymousAttribute>().Any();
        if (allowAnonymous) return;

        // authorization
        var user = (ApplicationUser) context.HttpContext.Items[nameof(ApplicationUser)];
        if (user == null)
            context.Result = new JsonResult(new {message = "Unauthorized"})
                {StatusCode = StatusCodes.Status401Unauthorized};
    }
}
