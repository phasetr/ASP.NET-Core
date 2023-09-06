using BlazorJwtAuth.Client.Common.Library;

namespace BlazorJwtAuth.Test.Client.Unit.Helpers;

public static class Constants
{
    public static readonly AppSettings AppSettings = new()
    {
        ApiBaseAddress = "http://localhost"
    };
}
