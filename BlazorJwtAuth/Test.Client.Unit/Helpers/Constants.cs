using Common.Helpers;

namespace Test.Client.Unit.Helpers;

public static class Constants
{
    public static readonly AppSettings AppSettings = new()
    {
        ApiBaseAddress = "http://localhost"
    };
}
