using BlazorJwtAuth.Client;
using BlazorJwtAuth.Client.Library;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(p => p.GetRequiredService<IConfiguration>().Get<AppSettings>());
builder.Services.AddScoped(_ => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
builder.Services.AddScoped<CookieStorageAccessor>();

await builder.Build().RunAsync();
