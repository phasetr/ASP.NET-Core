using BlazorJwtAuth.WebApi.Service.Services.Interfaces;

namespace BlazorJwtAuth.WebApi.Service.Services;

public class PtDateTime : IPtDateTime
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}
