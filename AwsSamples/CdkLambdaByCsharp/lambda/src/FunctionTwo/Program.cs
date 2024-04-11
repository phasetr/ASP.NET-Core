var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UsePathBase(new PathString("/function-two"));
app.UseRouting();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda - Function Two!\n");
app.MapGet("/add/{x:int}/{y:int}", (ILogger<Program> logger, int x, int y) =>
{
    logger.LogInformation("{X} plus {Y} is {XY}", x, y, x + y);
    return $"{x} + {y} = {x + y}\n";
});
app.MapGet("/subtract/{x:int}/{y:int}", (ILogger<Program> logger, int x, int y) =>
{
    logger.LogInformation("{X} subtract {Y} is {XY}", x, y, x - y);
    return $"{x} - {y} = {x - y}\n";
});
app.MapGet("/multiply/{x:int}/{y:int}", (ILogger<Program> logger, int x, int y) =>
{
    logger.LogInformation("{X} multiply {Y} is {XY}", x, y, x * y);
    return $"{x} * {y} = {x * y}\n";
});
app.MapGet("/divide/{x:int}/{y:int}", (ILogger<Program> logger, int x, int y) =>
{
    if (y == 0)
    {
        logger.LogError("Cannot divide by zero");
        return "Cannot divide by zero\n";
    }

    logger.LogInformation("{X} divide {Y} is {XY}", x, y, x / y);
    return $"{x} / {y} = {x / y}\n";
});

app.Run();
