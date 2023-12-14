using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Common;
using OpenAI_API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// パラメータストアからAPIキーを取得
var request = new GetParameterRequest
{
    Name = "OPENAI_API_KEY",
    WithDecryption = true
};
var client = new AmazonSimpleSystemsManagementClient();
var response = await client.GetParameterAsync(request);
var apiKey = response.Parameter.Value;

// DI
builder.Services.AddScoped<IOpenAiRequest, OpenAiRequest>(_ =>
{
    var api = new OpenAIAPI(apiKey);
    return new OpenAiRequest(api);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda!\n");
app.MapGet("/one", () => "Function One: /one\n");

app.Run();
