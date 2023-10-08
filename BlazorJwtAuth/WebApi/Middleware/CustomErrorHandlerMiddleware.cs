using System;
using System.Threading.Tasks;
using Common.Dto;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebApi.Middleware;

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
                Message = ex.Message
            };

            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        }
    }
}
