using Blazor;
using Common.Helpers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClientの設定
builder.Services.AddSingleton(p => p.GetRequiredService<IConfiguration>().Get<AppSettings>());
var apiBaseAddress = builder.Configuration[nameof(AppSettings.ApiBaseAddress)];
if (string.IsNullOrWhiteSpace(apiBaseAddress))
    throw new Exception("ApiBaseAddress is not set in appsettings.json");
builder.Services.AddScoped(_ => new HttpClient {BaseAddress = new Uri(apiBaseAddress)});

await builder.Build().RunAsync();
