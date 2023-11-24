using System.Text.Json;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Api.Services;
using Api.Services.Interfaces;
using Common.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Logger
builder.Logging.ClearProviders().AddJsonConsole();

// JWT Authentication
// https://copyprogramming.com/howto/net-core-web-api-jwt-authentication-code-example#how-to-validate-aws-cognito-jwt-in-net-core-web-api-using-addjwtbearer
builder.Services.AddAuthentication(options =>
    {
        // JWT認証しか使わないためJWT認証をデフォルトにする
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var envRegion = Environment.GetEnvironmentVariable("REGION");
        var envCognitoUserPoolId = Environment.GetEnvironmentVariable("COGNITO_USER_POOL_ID");
        var envClientId = Environment.GetEnvironmentVariable("CLIENT_ID");
        options.Authority = $"https://cognito-idp.{envRegion}.amazonaws.com/{envCognitoUserPoolId}";
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKeyResolver = (_, _, _, parameters) =>
            {
                var keyUrl = parameters.ValidIssuer + "/.well-known/jwks.json";
                var client = new HttpClient();
                return client.GetFromJsonAsync<IEnumerable<SecurityKey>>(keyUrl).Result;
            },
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidAudience = envClientId,
            ValidIssuer = $"https://cognito-idp.{envRegion}.amazonaws.com/{envCognitoUserPoolId}"
        };
    });

// CORS設定
var clientUrl = builder.Configuration["CLIENT_URL"];
const string corsPolicy = "My Policy";
builder.Services.AddCors(o => o.AddPolicy(
    corsPolicy,
    corsPolicyBuilder =>
    {
        corsPolicyBuilder
            .WithOrigins(clientUrl)
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    }));

// Add services to the container.
builder.Services
    .AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; });

// DynamoDB
var region = Environment.GetEnvironmentVariable("AWS_REGION") ?? RegionEndpoint.APNortheast1.SystemName;
var amazonDynamoDbConfig = new AmazonDynamoDBConfig
{
    RegionEndpoint = RegionEndpoint.GetBySystemName(region)
};
// 開発環境だけ`ServiceURL`を`DynamoDB Local`に設定する
// `ServiceURL`は`compose.yml`で設定
if (builder.Environment.IsDevelopment()) amazonDynamoDbConfig.ServiceURL = "http://localhost:8000";
// デバッグ用
// Console.WriteLine($"DynamoDB Region: {region}");
// Console.WriteLine($"DynamoDB ServiceURL: {amazonDynamoDbConfig.ServiceURL}");
builder.Services
    .AddSingleton<IAmazonDynamoDB>(new AmazonDynamoDBClient(amazonDynamoDbConfig))
    .AddScoped<IDynamoDBContext, DynamoDBContext>()
    .AddScoped<IBookService, BookService>();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo {Title = "My API", Version = "v1"});
    // JWT認証用の設定
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        // トークンを指定するための入力フィールドを追加
        c.OAuthClientId("swagger-client-id");
        c.OAuthClientSecret("swagger-client-secret");
        c.OAuthAppName("Swagger UI");
        c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
    });
}

app.UseHttpsRedirection();
app.UseCors(corsPolicy);
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.MapControllers();

app.MapGet("/", [AllowAnonymous](ILogger<Program> logger) =>
    {
        logger.LogInformation("Welcome to running ASP.NET Core Minimal API on AWS Lambda, CORS: {ClientUrl}",
            clientUrl);
        return new ResponseBaseDto
        {
            Message = $"Welcome to running ASP.NET Core Minimal API on AWS Lambda, CORS: {clientUrl}",
            Succeeded = true
        };
    }
);

app.Run();

// cf. https://stackoverflow.com/questions/69991983/deps-file-missing-for-dotnet-6-integration-tests
// テストを動作させるために追加
// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program
{
}