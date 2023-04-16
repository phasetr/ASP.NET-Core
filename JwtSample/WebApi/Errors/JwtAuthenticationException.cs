using System.Globalization;

namespace WebApi.Errors;

// custom exception class for throwing application specific exceptions (e.g. for validation) 
// that can be caught and handled within the application
public class JwtAuthenticationException : Exception
{
    public JwtAuthenticationException()
    {
    }

    public JwtAuthenticationException(string message) : base(message)
    {
    }

    public JwtAuthenticationException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}
