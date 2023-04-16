using System.Net;
using System.Text.Json;

namespace WebApi.Errors;

public class JwtAuthenticationErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public JwtAuthenticationErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            response.StatusCode = error switch
            {
                JwtAuthenticationException _ =>
                    // custom application error
                    (int) HttpStatusCode.BadRequest,
                KeyNotFoundException _ =>
                    // not found error
                    (int) HttpStatusCode.NotFound,
                _ => (int) HttpStatusCode.InternalServerError
            };

            var result = JsonSerializer.Serialize(new {message = error.Message});
            await response.WriteAsync(result);
        }
    }
}
