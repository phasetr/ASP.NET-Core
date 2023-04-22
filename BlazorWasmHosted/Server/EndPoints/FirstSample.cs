namespace BlazorWasmHosted.Server.EndPoints;

public static class FirstSample
{
    public static void MapFirstSample(this WebApplication app)
    {
        app.MapGet("/firstsample", () => "Hello World!");
    }
}
