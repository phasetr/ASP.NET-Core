using System;
using System.Threading.Tasks;
using BlazorJwtAuth.Common.Dto;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BlazorJwtAuth.WebApi.Middleware;

public class CustomErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public CustomErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var errorResponse = new ErrorResponse
            {
                Message = "An unexpected error occurred."
            };

            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        }
    }
}
