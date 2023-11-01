using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.AspNetCore.Mvc;

namespace Function.Controllers;

[Route("move")]
[ApiController]
public class MoveController : ControllerBase
{
    private readonly IAmazonDynamoDB _amazonDynamoDb;
    private readonly ILogger<MoveController> _logger;

    public MoveController(ILogger<MoveController> logger, IAmazonDynamoDB amazonDynamoDb)
    {
        _logger = logger;
        _amazonDynamoDb = amazonDynamoDb;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id)
    {
        _logger.LogInformation("👺Accessed: /move/{Id}", id);
        var tableName = Environment.GetEnvironmentVariable("TABLE_NAME");
        var table = Table.LoadTable(_amazonDynamoDb, tableName);
        var response = await table.GetItemAsync(id);
        if (response is null) return BadRequest($"No redirect found for {id}");
        return RedirectPermanent(response["targetUrl"].AsString());
    }
}
