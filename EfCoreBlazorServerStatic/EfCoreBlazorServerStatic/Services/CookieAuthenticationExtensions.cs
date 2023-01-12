using System.Linq.Expressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EfCoreBlazorServerStatic.Services;

public static class CookieAuthenticationExtensions
{
    public static void DisableRedirectForPath(
        this CookieAuthenticationEvents events,
        Expression<Func<CookieAuthenticationEvents,
            Func<RedirectContext<CookieAuthenticationOptions>, Task>>> expr,
        string path, int statusCode)
    {
        var propertyName = ((MemberExpression) expr.Body).Member.Name;
        var oldHandler = expr.Compile().Invoke(events);

        Func<RedirectContext<CookieAuthenticationOptions>, Task> newHandler
            = context =>
            {
                if (context.Request.Path.StartsWithSegments(path))
                    context.Response.StatusCode = statusCode;
                else
                    oldHandler(context);
                return Task.CompletedTask;
            };

        typeof(CookieAuthenticationEvents).GetProperty(propertyName)?
            .SetValue(events, newHandler);
    }
}