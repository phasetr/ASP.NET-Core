using System.Security.Claims;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Api.Services;
using AspNetCore.Identity.AmazonDynamoDB;
using Common;
using Common.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

// Establish cookie authentication
// builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();
// Configure authorization
builder.Services.AddAuthorizationBuilder();

// // Add the database (in memory for the sample)
// builder.Services.AddDbContext<AppDbContext>(
//     options => options.UseInMemoryDatabase("AppDb"));
// // Add identity and opt-in to endpoints
// builder.Services
//     .AddIdentityCore<ApplicationUser>()
//     .AddRoles<ApplicationRole>()
//     .AddRoleManager<RoleManager<ApplicationRole>>()
//     .AddEntityFrameworkStores<AppDbContext>()
//     .AddApiEndpoints();

// DynamoDB
var region = Environment.GetEnvironmentVariable("AWS_REGION") ?? RegionEndpoint.APNortheast1.SystemName;
// 開発環境だけ`ServiceURL`を`DynamoDB Local`に設定する
// `ServiceURL`は`compose.yml`で設定
var amazonDynamoDbConfig = builder.Environment.IsDevelopment()
    ? new AmazonDynamoDBConfig
    {
        RegionEndpoint = RegionEndpoint.GetBySystemName(region),
        ServiceURL = "http://localhost:8000"
    }
    : new AmazonDynamoDBConfig { RegionEndpoint = RegionEndpoint.GetBySystemName(region) };
builder.Services
    .AddDefaultAWSOptions(builder.Configuration.GetAWSOptions())
    .AddSingleton<IAmazonDynamoDB>(new AmazonDynamoDBClient(amazonDynamoDbConfig));
builder.Services.AddTransient<IEmailSender, NoOpEmailSender>();
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();
builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>()
    .AddDefaultUI()
    .AddDefaultTokenProviders()
    .AddDynamoDbStores()
    .Configure(options =>
        options.DefaultTableName = builder.Environment.IsDevelopment()
            ? MyConstants.DynamoDbLocalTableName
            : MyConstants.DynamoDbDevTableName);
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
});
builder.Services.AddSingleton(TimeProvider.System);
// Add a CORS policy for the client
builder.Services.AddCors(
    options => options.AddPolicy(
        "wasm",
        policy => policy.WithOrigins([
                builder.Configuration["Url:FrontendUrl"] ?? "https://localhost:6500",
                "https://localhost:6500"
            ])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()));

builder.Services.Configure<MyDynamoDbSettings>(builder.Configuration.GetSection("DynamoDbSettings"));
builder.Services.AddScoped<IBookService, BookService>();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    Console.WriteLine("👺Development mode");
    // Seed the database
    // Console.WriteLine("👺Development mode detected. Adding seed data.");
    // await using var scope = app.Services.CreateAsyncScope();
    // await SeedData.InitializeAsync(scope.ServiceProvider);
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // 必要に応じて本番環境でも公開して調べよう
    app.UseSwagger();
    app.UseSwaggerUI();
    Console.WriteLine("👺Production mode");
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Activate the CORS policy
app.UseCors("wasm");

// Enable authentication and authorization after CORS Middleware
// processing (UseCors) in case the Authorization Middleware tries
// to initiate a challenge before the CORS Middleware has a chance
// to set the appropriate headers.
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapIdentityApi<ApplicationUser>();

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

app.MapGet("/", () =>
{
    var env = builder.Environment.IsDevelopment() ? "local" : "Production";
    return Results.Text($"Hello, world in {env}!\n");
});
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

app.MapGet("/user/{email}", async (string email, UserManager<ApplicationUser> userManager) =>
{
    var user = await userManager.FindByEmailAsync(email);
    if (user == null) return Results.NotFound();
    return Results.Ok(new
    {
        user.Id,
        user.UserName,
        user.Email
    });
});

app.MapGet("/book", async (IBookService bookService) =>
{
    var responseDto = await bookService.GetListAsync();
    return Results.Ok(responseDto);
});
app.MapGet("/book/{id}", async (string id, IBookService bookService) =>
{
    var responseDto = await bookService.GetItemAsync(id);
    return responseDto == null ? Results.NotFound() : Results.Ok(responseDto);
});
app.MapPut("/book", async ([FromBody] BookDto dto, IBookService bookService) =>
{
    var bookId = await bookService.SaveItemAsync(dto);
    return Results.Ok(bookId);
});

app.MapPost("/data-processing-1", ([FromBody] FormModel model) =>
    Results.Text($"{model.Message.Length} characters"))
    .RequireAuthorization();

app.MapPost("/data-processing-2", ([FromBody] FormModel model) =>
    Results.Text($"{model.Message.Length} characters"))
    .RequireAuthorization(policy => policy.RequireRole("Manager"));

app.Run();

// Identity user
internal class ApplicationUser : DynamoDbUser
{
    // public new IEnumerable<ApplicationRole>? Roles { get; set; }
}

// Identity role
internal class ApplicationRole : DynamoDbRole
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
