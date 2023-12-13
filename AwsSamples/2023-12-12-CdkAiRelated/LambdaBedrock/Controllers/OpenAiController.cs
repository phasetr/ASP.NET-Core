using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using LambdaBedrock.Dtos;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API;
using OpenAI_API.Chat;

namespace LambdaBedrock.Controllers;

[ApiController]
[Route("openai")]
public class OpenAiController : ControllerBase
{
    private readonly ILogger<OpenAiController> _logger;

    public OpenAiController(ILogger<OpenAiController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<ResponseBaseDto> PostAsync(OpenAiPostRequestDto dto)
    {
        _logger.LogInformation("OpenAiController.PostAsync");

        if(!ModelState.IsValid)
        {
            return new ResponseBaseDto
            {
                Message = "Invalid request",
                Succeeded = false
            };
        }

        var client = new AmazonSimpleSystemsManagementClient();
        var request = new GetParameterRequest
        {
            Name = "OPENAI_API_KEY",
            WithDecryption = true
        };

        try
        {
            var response = await client.GetParameterAsync(request);
            var apiKey = response.Parameter.Value;
            var api = new OpenAIAPI(apiKey);
            var message = new ChatMessage
            {
                Role = ChatMessageRole.User,
                TextContent = "富士山の高さは何メートルですか？"
            };
            var result = await api.Chat.CreateChatCompletionAsync(
                model: "gpt-3.5-turbo",
                messages: new List<ChatMessage> {message});
            var textContent = result.Choices[0].Message.TextContent;
            _logger.LogInformation("{T}", textContent);

            return new ResponseBaseDto
            {
                Message = textContent,
                Succeeded = true
            };
        }
        catch (AmazonSimpleSystemsManagementException e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            return new ResponseBaseDto
            {
                Message = e.Message,
                Succeeded = false
            };
        }
    }

    [HttpGet]
    public async Task<ResponseBaseDto> GetAsync()
    {
        _logger.LogInformation("OpenAiController.GetAsync");

        var client = new AmazonSimpleSystemsManagementClient();
        var request = new GetParameterRequest
        {
            Name = "OPENAI_API_KEY",
            WithDecryption = true
        };

        try
        {
            var response = await client.GetParameterAsync(request);
            var apiKey = response.Parameter.Value;
            var api = new OpenAIAPI(apiKey);
            var message = new ChatMessage
            {
                Role = ChatMessageRole.User,
                TextContent = "富士山の高さは何メートルですか？"
            };
            var result = await api.Chat.CreateChatCompletionAsync(
                model: "gpt-3.5-turbo",
                messages: new List<ChatMessage> {message});
            var textContent = result.Choices[0].Message.TextContent;
            _logger.LogInformation("{T}", textContent);

            return new ResponseBaseDto
            {
                Message = textContent,
                Succeeded = true
            };
        }
        catch (AmazonSimpleSystemsManagementException e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            return new ResponseBaseDto
            {
                Message = e.Message,
                Succeeded = false
            };
        }
    }
}
