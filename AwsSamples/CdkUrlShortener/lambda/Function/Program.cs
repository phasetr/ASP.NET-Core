using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Get the AWS profile information from configuration providers
var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda - Function One!");
app.MapGet("/minimal/one", () => "Function One: /one");
app.MapGet("/minimal/add/{x:int}/{y:int}", (ILogger<Program> logger, int x, int y) =>
{
    logger.LogInformation("{X} plus {Y} is {XY}", x, y, x + y);
    return $"{x} + {y} = {x + y}";
});
app.MapGet("/minimal/subtract/{x:int}/{y:int}", (ILogger<Program> logger, int x, int y) =>
{
    logger.LogInformation("{X} subtract {Y} is {XY}", x, y, x - y);
    return $"{x} - {y} = {x - y}";
});
app.MapGet("/minimal/multiply/{x:int}/{y:int}", (ILogger<Program> logger, int x, int y) =>
{
    logger.LogInformation("{X} multiply {Y} is {XY}", x, y, x * y);
    return $"{x} * {y} = {x * y}";
});
app.MapGet("/minimal/divide/{x:int}/{y:int}", (ILogger<Program> logger, int x, int y) =>
{
    if (y == 0)
    {
        logger.LogError("Cannot divide by zero");
        return "Cannot divide by zero";
    }

    logger.LogInformation("{X} divide {Y} is {XY}", x, y, x / y);
    return $"{x} / {y} = {x / y}";
});

app.Run();
