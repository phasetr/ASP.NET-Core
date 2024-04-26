using BlazorWasmAuth.Components;
using BlazorWasmAuth.Identity;
using Common;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// register the cookie handler
builder.Services.AddTransient<CookieHandler>();

// set up authorization
builder.Services.AddAuthorizationCore();

// register the custom state provider
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();

// register the account management interface
builder.Services.AddScoped(
    sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

// set base address for default host
var baseAddress = builder.Configuration["Url:BackendUrl"] ?? "http://localhost:5500";
builder.Services.AddScoped(_ =>
    new HttpClient { BaseAddress = new Uri(baseAddress!) });

// configure client for auth interactions
builder.Services.AddHttpClient(
        "Auth",
        opt => opt.BaseAddress = new Uri(baseAddress!))
    .AddHttpMessageHandler<CookieHandler>();

builder.Services.Configure<MyUrl>(builder.Configuration.GetSection("Url"));

await builder.Build().RunAsync();
