using Common.Services.Interfaces;

namespace Common.Services;

public class PtDateTime : IPtDateTime
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}
