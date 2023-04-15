using BlazorWebAssemblyWithRazorPages.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlazorWebAssemblyWithRazorPages.Server.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // skip authorization if action is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;

        // authorization
        if (context.HttpContext.Items["User"] is not ApplicationUser _)
            context.Result = new JsonResult(new {message = "Unauthorized"})
                {StatusCode = StatusCodes.Status401Unauthorized};
    }
}
