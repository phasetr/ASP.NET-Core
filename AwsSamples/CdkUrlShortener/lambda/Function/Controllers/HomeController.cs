using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.AspNetCore.Mvc;

namespace Function.Controllers;

[Route("/")]
[ApiController]
public class HomeController : ControllerBase
{
    private readonly IAmazonDynamoDB _amazonDynamoDb;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IAmazonDynamoDB amazonDynamoDb, ILogger<HomeController> logger)
    {
        _amazonDynamoDb = amazonDynamoDb;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(string? targetUrl)
    {
        _logger.LogInformation("👺 Accessed: /?targetUrl={TargetUrl}", targetUrl);
        if (targetUrl is null) return Ok("usage: /?targetUrl=<URL>");

        _logger.LogInformation("👺 1");
        var tableName = Environment.GetEnvironmentVariable("TABLE_NAME");
        if (tableName is null)
        {
            _logger.LogError("👺 TABLE_NAME is null");
            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = "An error occurred while processing your request.",
                Status = StatusCodes.Status500InternalServerError,
                Instance = HttpContext.Request.Path
            };
            return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
        }

        _logger.LogInformation("👺 table name: {TableName}", tableName);
        _logger.LogInformation("👺 2");
        var table = Table.LoadTable(_amazonDynamoDb, tableName);
        var id = Guid.NewGuid().ToString()[..8];
        await table.PutItemAsync(new Document
        {
            {"id", id},
            {
                "targetUrl", targetUrl
            }
        });
        var url = $"https://{HttpContext.Request.Host}/prod/move/{id}";
        _logger.LogInformation("👺 id: {Id}, Created URL: {Url}", id, url);
        return Ok($"Created URL: {url}");
    }
}
