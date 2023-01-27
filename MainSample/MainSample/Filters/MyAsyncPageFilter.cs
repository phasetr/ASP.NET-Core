using Microsoft.AspNetCore.Mvc.Filters;

namespace MainSample.Filters;

public class MyAsyncPageFilter : IAsyncPageFilter
{
    private readonly ILogger _logger;

    public MyAsyncPageFilter(ILogger logger)
    {
        _logger = logger;
    }

    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
    {
        var httpContext = context.HttpContext;
        _logger.LogTrace(
            "HttpType: {RequestMethod}|Path :{RequestPath}\\tUserAgent: {S}\\tPage: {Name}\\tHandler: {MethodInfoName}",
            httpContext.Request.Method,
            httpContext.Request.Path,
            httpContext.Request.Headers["User-Agent"].ToString(),
            context.HandlerInstance.GetType().Name,
            context.HandlerMethod?.MethodInfo.Name);
        return Task.CompletedTask;
    }

    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context,
        PageHandlerExecutionDelegate next)
    {
        await next.Invoke();
    }
}