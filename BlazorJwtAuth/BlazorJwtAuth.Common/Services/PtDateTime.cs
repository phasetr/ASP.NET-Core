using BlazorJwtAuth.Common.Services.Interfaces;

namespace BlazorJwtAuth.Common.Services;

public class PtDateTime : IPtDateTime
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}
