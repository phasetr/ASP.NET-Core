namespace Common.Services.Interfaces;

public interface IPtDateTime
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}
