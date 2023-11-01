using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 開発時のスキャフォールドやマイグレーションで何度となくエラーになったため、一旦コメントアウト
// var clusterSecretEnvironmentVariable = Environment.GetEnvironmentVariable("APCLUSTER_SECRET");
// var connectionString = DbConnectionStringHelper.GetConnectionString(clusterSecretEnvironmentVariable);
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Console.WriteLine("👺 connectionString: " + connectionString);
// 
// try
// {
//     const string rdsSecretName = "cdk-app-runner-rds-db-cluster/pgadmin";
//     const string region = "ap-northeast-1";
//     var client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));
//     var request = new GetSecretValueRequest
//     {
//         SecretId = rdsSecretName
//     };
//     var response = await client.GetSecretValueAsync(request);
//     if (response.SecretString is not null)
//     {
//         connectionString = response.SecretString;
//         Console.WriteLine("👺 connectionString from Secrets Manager: " + connectionString);
//     }
// }
// catch (Exception e)
// {
//     Console.WriteLine("👺 Not in AWS Environment");
//     Console.WriteLine(e.StackTrace);
// }
//
// builder.Services.AddDbContext<ApplicationDbContext>(options => { options.UseNpgsql(connectionString); });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", (ILogger<Program> logger) =>
{
    logger.LogInformation("👺 access to /");
    return "This is a GET.";
});
// app.MapGet("/v1/index", (ILogger<Program> logger) =>
// {
//     logger.LogInformation("👺 access to /v1/index");
//     return "GET: /v1/index!";
// });
// app.MapGet("/v1/test1", (ILogger<Program> logger) =>
// {
//     logger.LogInformation("👺 access to /v1/test1");
//     return "GET: /v1/test1";
// });
// app.MapGet("/v1/test2", (ILogger<Program> logger) =>
// {
//     logger.LogInformation("👺 access to /v1/test2");
//     return "GET: /v1/test2";
// });
// app.MapGet("/v1/course", async (ILogger<Program> logger, ApplicationDbContext context) =>
// {
//     logger.LogInformation("👺 access to /v1/course");
//     var courses = await context.Course.ToListAsync();
//     return Results.Ok(courses);
// }).Produces<List<Course>>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
