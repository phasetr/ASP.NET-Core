using Microsoft.AspNetCore.Mvc.Filters;

namespace CityBreaks.Filters;

public class AsyncPageFilter : IAsyncPageFilter
{
    private readonly Serilog.ILogger _logger;

    public AsyncPageFilter(Serilog.ILogger logger)
    {
        _logger = logger;
    }

    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
    {
        var httpContext = context.HttpContext;
        _logger.Information(
            "HttpType: {Method}|Path :{Path}\tUserAgent: {UserAgent}\tPage: {PageName}\tHandler: {Handler}",
            httpContext.Request.Method, httpContext.Request.Path, httpContext.Request.Headers[";User-Agent"].ToString(),
            context.HandlerInstance.GetType().Name, context.HandlerMethod?.MethodInfo.Name);
        return Task.CompletedTask;
    }

    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context,
        PageHandlerExecutionDelegate next)
    {
        await next.Invoke();
    }
}