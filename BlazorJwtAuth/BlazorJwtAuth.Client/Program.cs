using Blazored.LocalStorage;
using BlazorJwtAuth.Client;
using BlazorJwtAuth.Client.Common.Library;
using BlazorJwtAuth.Client.Service.Classes;
using BlazorJwtAuth.Client.Service.Services;
using BlazorJwtAuth.Client.Service.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(p => p.GetRequiredService<IConfiguration>().Get<AppSettings>());
var apiRoot = builder.Configuration[nameof(AppSettings.ApiBaseAddress)];
if (string.IsNullOrWhiteSpace(apiRoot))
    throw new Exception("ApiBaseAddress is not set in appsettings.json");
builder.Services.AddHttpClient<AuthenticationHttpClient>(client =>
    client.BaseAddress = new Uri(apiRoot));

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddScoped(_ => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
builder.Services.AddScoped<CookieStorageAccessor>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddSingleton<IConsumptionTaxCalculator, ConsumptionTaxCalculator>();

await builder.Build().RunAsync();
