namespace BlazorJwtAuth.WebApi.Service.Services.Interfaces;

public interface IPtDateTime
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}
