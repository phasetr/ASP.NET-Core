using System.Security.Claims;
using Backend;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

// Establish cookie authentication
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();

// Configure authorization
builder.Services.AddAuthorizationBuilder();

// Add the database (in memory for the sample)
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseInMemoryDatabase("AppDb"));

// Add identity and opt-in to endpoints
builder.Services.AddIdentityCore<AppUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

// Add a CORS policy for the client
builder.Services.AddCors(
    options => options.AddPolicy(
        "wasm",
        policy => policy.WithOrigins([
                builder.Configuration["BackendUrl"] ?? "http://localhost:5266",
                builder.Configuration["FrontendUrl"] ?? "http://localhost:5170"
            ])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()));

// Add services to the container
builder.Services.AddEndpointsApiExplorer();

// Add NSwag services
builder.Services.AddOpenApiDocument();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    Console.WriteLine("👺Development mode detected. Adding seed data.");
    // Seed the database
    await using var scope = app.Services.CreateAsyncScope();
    await SeedData.InitializeAsync(scope.ServiceProvider);
    // Add OpenAPI/Swagger generator and the Swagger UI
    app.UseOpenApi();
    app.UseSwaggerUi();
}

// Create routes for the identity endpoints
app.MapIdentityApi<AppUser>();

// Activate the CORS policy
app.UseCors("wasm");

// Enable authentication and authorization after CORS Middleware
// processing (UseCors) in case the Authorization Middleware tries
// to initiate a challenge before the CORS Middleware has a chance
// to set the appropriate headers.
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Provide an end point to clear the cookie for logout
//
// For more information on the logout endpoint and antiforgery, see:
// https://learn.microsoft.com/aspnet/core/blazor/security/webassembly/standalone-with-identity#antiforgery-support
app.MapPost("/logout", async (SignInManager<AppUser> signInManager, [FromBody] object? empty) =>
{
    if (empty is null) return Results.Unauthorized();
    await signInManager.SignOutAsync();
    return Results.Ok();
}).RequireAuthorization();

app.UseHttpsRedirection();

app.MapGet("/", () => Results.Text("Hello, world!\n"));
app.MapGet("/roles", (ClaimsPrincipal user) =>
{
    if (user.Identity is null || !user.Identity.IsAuthenticated) return Results.Unauthorized();
    var identity = (ClaimsIdentity)user.Identity;
    var roles = identity.FindAll(identity.RoleClaimType)
        .Select(c =>
            new
            {
                c.Issuer,
                c.OriginalIssuer,
                c.Type,
                c.Value,
                c.ValueType
            });
    return TypedResults.Json(roles);
}).RequireAuthorization();

app.MapPost("/data-processing-1", ([FromBody] FormModel model) =>
    Results.Text($"{model.Message.Length} characters"))
    .RequireAuthorization();

app.MapPost("/data-processing-2", ([FromBody] FormModel model) =>
    Results.Text($"{model.Message.Length} characters"))
    .RequireAuthorization(policy => policy.RequireRole("Manager"));

app.Run();

// Identity user
internal class AppUser : IdentityUser
{
    public IEnumerable<IdentityRole>? Roles { get; set; }
}

// Identity database
internal class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
{
}

// Example form model
internal class FormModel
{
    public string Message { get; set; } = string.Empty;
}
