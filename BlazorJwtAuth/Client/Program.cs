using Blazored.LocalStorage;
using Client;
using Client.Classes;
using Client.Helpers;
using Client.Services;
using Client.Services.Interfaces;
using Common.Helpers;
using Common.Services;
using Common.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(p => p.GetRequiredService<IConfiguration>().Get<AppSettings>());
var apiBaseAddress = builder.Configuration[nameof(AppSettings.ApiBaseAddress)];
if (string.IsNullOrWhiteSpace(apiBaseAddress))
    throw new Exception("ApiBaseAddress is not set in appsettings.json");
builder.Services.AddHttpClient<HttpClient>(client => client.BaseAddress = new Uri(apiBaseAddress));
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddScoped<CookieStorageAccessor>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped(_ => new HttpClient {BaseAddress = new Uri(apiBaseAddress)});
builder.Services.AddScoped<IAuthenticationHttpClientService, AuthenticationHttpClientService>();
builder.Services.AddScoped<IHomeHttpClientService, HomeHttpClientService>();
builder.Services.AddScoped<IPtDateTime, PtDateTime>();
builder.Services.AddScoped<ISecuredHttpClientService, SecuredHttpClientService>();
builder.Services.AddScoped<ITokenHttpClientService, TokenHttpClientService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IWeatherForecastHttpClientService, WeatherForecastHttpClientService>();
builder.Services.AddSingleton<IConsumptionTaxCalculator, ConsumptionTaxCalculator>();

await builder.Build().RunAsync();
