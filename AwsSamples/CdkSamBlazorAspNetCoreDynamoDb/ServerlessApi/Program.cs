using System.Text.Json;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.OpenApi.Models;
using ServerlessApi.Service.Interfaces;
using ServerlessApi.Service.Services;

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

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo {Title = "My API", Version = "v1"});
});

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.MapControllers();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");

app.Run();

// cf. https://stackoverflow.com/questions/69991983/deps-file-missing-for-dotnet-6-integration-tests
// テストを動作させるために追加
// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program
{
}
