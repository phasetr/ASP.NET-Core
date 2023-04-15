namespace BlazorWebAssemblyWithRazorPages.Server.Helpers;

public class AppSettings
{
    public string Secret { get; set; } = default!;

    // refresh token time to live (in days), inactive tokens are
    // automatically deleted from the database after this time
    public int RefreshTokenTtl { get; set; } = default!;
}
