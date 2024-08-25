using BlazorServerAuth0.Components;
using BlazorServerAuth0.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<QuizService>();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = _ => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

// Add authentication services
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddOpenIdConnect("Auth0", options =>
    {
        // Set the authority to your Auth0 domain
        options.Authority = $"https://{configuration["Auth0:Domain"]}";

        // Configure the Auth0 Client ID and Client Secret
        options.ClientId = configuration["Auth0:ClientId"];
        options.ClientSecret = configuration["Auth0:ClientSecret"];

        // Set response type to code
        options.ResponseType = "code";

        // Configure the scope
        options.Scope.Clear();
        options.Scope.Add("openid");

        // Set the callback path, so Auth0 will call back to http://localhost:3000/callback
        // Also ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard
        options.CallbackPath = new PathString("/callback");

        // Configure the Claims Issuer to be Auth0
        options.ClaimsIssuer = "Auth0";

        options.Events = new OpenIdConnectEvents
        {
            // handle the logout redirection
            OnRedirectToIdentityProviderForSignOut = context =>
            {
                var logoutUri =
                    $"https://{configuration["Auth0:Domain"]}/v2/logout?client_id={configuration["Auth0:ClientId"]}";

                var postLogoutUri = context.Properties.RedirectUri;
                if (!string.IsNullOrEmpty(postLogoutUri))
                {
                    if (postLogoutUri.StartsWith("/"))
                    {
                        // transform to absolute
                        var request = context.Request;
                        postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                    }

                    logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                }

                context.Response.Redirect(logoutUri);
                context.HandleResponse();

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.MapBlazorHub();

// エンドポイントの定義
app.MapGet("/api", () => "Hello World!");
app.MapGet("/api/login", async (HttpContext context, string redirectUri) =>
{
    var authProperties = new AuthenticationProperties
    {
        RedirectUri = redirectUri
    };
    await context.ChallengeAsync("Auth0", authProperties);
});
app.MapGet("/api/logout", async (HttpContext httpContext) =>
{
    await httpContext.SignOutAsync("Auth0", new AuthenticationProperties { RedirectUri = "/" });
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/");
});

app.Run();
