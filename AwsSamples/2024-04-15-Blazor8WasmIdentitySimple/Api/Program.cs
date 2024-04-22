using System.Security.Claims;
using Api;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

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
builder.Services
    .AddIdentityCore<ApplicationUser>()
    .AddRoles<ApplicationRole>()
    .AddRoleManager<RoleManager<ApplicationRole>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

// // DynamoDB
// var region = Environment.GetEnvironmentVariable("AWS_REGION") ?? RegionEndpoint.APNortheast1.SystemName;
// // 開発環境だけ`ServiceURL`を`DynamoDB Local`に設定する
// // `ServiceURL`は`compose.yml`で設定
// var amazonDynamoDbConfig = builder.Environment.IsDevelopment()
//     ? new AmazonDynamoDBConfig
//     {
//         RegionEndpoint = RegionEndpoint.GetBySystemName(region),
//         ServiceURL = "http://localhost:8000"
//     }
//     : new AmazonDynamoDBConfig { RegionEndpoint = RegionEndpoint.GetBySystemName(region) };
// builder.Services.AddSingleton<IAmazonDynamoDB>(new AmazonDynamoDBClient(amazonDynamoDbConfig));
// builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();
// builder.Services
//     .AddIdentityCore<DynamoDbUser>()
//     .AddRoles<DynamoDbRole>()
//     .AddDynamoDbStores()
//     .Configure(options =>
//     {
//         options.BillingMode = BillingMode.PROVISIONED; // Default is BillingMode.PAY_PER_REQUEST
//         options.ProvisionedThroughput = new ProvisionedThroughput
//         {
//             ReadCapacityUnits = 5, // Default is 1
//             WriteCapacityUnits = 5, // Default is 1
//         };
//         options.DefaultTableName = "my-custom-identity-table-name"; // Default is identity
//     });

// Add a CORS policy for the client
builder.Services.AddCors(
    options => options.AddPolicy(
        "wasm",
        policy => policy.WithOrigins([
                builder.Configuration["FrontendUrl"] ?? "https://localhost:6500",
                builder.Configuration["BackendUrl"] ?? "https://localhost:5500"
            ])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()));

// Add services to the container
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    Console.WriteLine("👺Development mode detected. Adding seed data.");
    // Seed the database
    await using var scope = app.Services.CreateAsyncScope();
    await SeedData.InitializeAsync(scope.ServiceProvider);
    // 必要に応じて本番環境でも公開して調べよう
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Create routes for the identity endpoints
app.MapIdentityApi<ApplicationUser>();

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
app.MapPost("/logout", async (SignInManager<ApplicationUser> signInManager, [FromBody] object? empty) =>
{
    if (empty is null) return Results.Unauthorized();
    await signInManager.SignOutAsync();
    return Results.Ok();
}).RequireAuthorization();

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
internal class ApplicationUser : IdentityUser
{
    public IEnumerable<ApplicationRole>? Roles { get; set; }
}

// Identity role
internal class ApplicationRole : IdentityRole
{
}

// Identity database
internal class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
}

// Example form model
internal class FormModel
{
    public string Message { get; set; } = string.Empty;
}
