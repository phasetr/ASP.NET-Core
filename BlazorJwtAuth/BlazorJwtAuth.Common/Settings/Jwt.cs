namespace BlazorJwtAuth.Common.Settings;

public class Jwt
{
    public string Key { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public double DurationInMinutes { get; set; }
}
