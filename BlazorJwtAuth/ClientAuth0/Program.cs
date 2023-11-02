using ClientAuth0;
using Common.Helpers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClientの設定
builder.Services.AddSingleton(p => p.GetRequiredService<IConfiguration>().Get<AppSettings>());
var apiBaseAddress = builder.Configuration[nameof(AppSettings.ApiBaseAddress)];
if (string.IsNullOrWhiteSpace(apiBaseAddress))
    throw new Exception("ApiBaseAddress is not set in appsettings.json");
// builder.Services.AddScoped(_ => new HttpClient {BaseAddress = new Uri(apiBaseAddress)});
// Microsoft.Extensions.Http
builder.Services.AddHttpClient("ServerAPI",
        client => client.BaseAddress = new Uri(apiBaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("ServerAPI"));

// Auth0
builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
    // 認証付きAPIを呼び出すための設定
    options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]);
});

await builder.Build().RunAsync();
