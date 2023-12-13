using Amazon.BedrockRuntime.Model;
using LambdaBedrock.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LambdaBedrock.Controllers;

[ApiController]
[Route("weather")]
public class BedrockController : ControllerBase
{
    private readonly ILogger<BedrockController> _logger;

    public BedrockController(ILogger<BedrockController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ResponseBaseDto Get()
    {
        _logger.LogInformation("BedrockController.Get");
        var prompt = "富士山の高さは何メートルですか？";

        // 各種パラメーターの指定
        var modelId = "ai21.j2-mid-v1";
        var accept = "application/json";
        var contentType = "application/json";

        // リクエストボディの指定
        var requestBody = new
        {
            prompt,
            max_tokens = 100,
            temperature = 0.7,
            top_p = 1.0
        };

        // TODO Bedrock RuntimeによるBedrock APIの呼び出し
        var request = new InvokeModelRequest
        {
            ModelId = modelId,
            Accept = accept,
            ContentType = contentType,
            Body = new MemoryStream()
        };

        return new ResponseBaseDto
        {
            Message = "OK",
            Succeeded = true
        };
    }
}
