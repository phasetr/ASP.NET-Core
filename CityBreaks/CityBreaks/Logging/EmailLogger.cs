using Microsoft.AspNetCore.Identity.UI.Services;

namespace CityBreaks.Logging;

public class EmailLogger : ILogger
{
    private readonly IEmailSender _emailSender;

    public EmailLogger(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel is LogLevel.Error or LogLevel.Critical;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
        Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;
        var htmlMessage = state + "<br><br>" +
                          exception?.Message + "<br><br>" +
                          exception?.StackTrace;
        Task.Run(() => SendLog(htmlMessage));
    }

    private async Task SendLog(string htmlMessage)
    {
        const string subject = "Error in application";
        const string to = "test@example.com";
        await _emailSender.SendEmailAsync(to, subject, htmlMessage);
    }
}