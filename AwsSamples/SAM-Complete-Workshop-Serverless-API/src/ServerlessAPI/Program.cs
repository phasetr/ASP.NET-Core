using System.Text.Json;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.OpenApi.Models;
using ServerlessAPI.Services;

var builder = WebApplication.CreateBuilder(args);

//Logger
builder.Logging
    .ClearProviders()
    .AddJsonConsole();

// Add services to the container.
builder.Services
    .AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; });

var region = Environment.GetEnvironmentVariable("AWS_REGION") ?? RegionEndpoint.USEast2.SystemName;
var amazonDynamoDbConfig = new AmazonDynamoDBConfig
{
    RegionEndpoint = RegionEndpoint.GetBySystemName(region)
};
// 開発環境だけ`ServiceURL`を`DynamoDB Local`に設定する
if (builder.Environment.IsDevelopment())
    // `ServiceURL`は`compose.yml`で設定
    amazonDynamoDbConfig.ServiceURL = "http://localhost:8000";
// デバッグ用
// Console.WriteLine($"DynamoDB Region: {region}");
// Console.WriteLine($"DynamoDB ServiceURL: {amazonDynamoDbConfig.ServiceURL}");
builder.Services
    .AddSingleton<IAmazonDynamoDB>(new AmazonDynamoDBClient(amazonDynamoDbConfig))
    .AddScoped<IDynamoDBContext, DynamoDBContext>()
    .AddScoped<IBookService, BookService>();

// Add AWS Lambda support. When running the application as an AWS Serverless application, Kestrel is replaced
// with a Lambda function contained in the Amazon.Lambda.AspNetCoreServer package, which marshals the request into the ASP.NET Core hosting framework.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo {Title = "My API", Version = "v1"});
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");

app.Run();
