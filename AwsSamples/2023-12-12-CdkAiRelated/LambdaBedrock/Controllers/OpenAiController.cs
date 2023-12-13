using Amazon.SimpleSystemsManagement;
using Common;
using LambdaBedrock.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LambdaBedrock.Controllers;

[ApiController]
[Route("openai")]
public class OpenAiController : ControllerBase
{
    private readonly ILogger<OpenAiController> _logger;
    private readonly IOpenAiRequest _openAiRequest;

    public OpenAiController(
        ILogger<OpenAiController> logger,
        IOpenAiRequest openAiRequest)
    {
        _logger = logger;
        _openAiRequest = openAiRequest;
    }

    [HttpPost]
    public async Task<ResponseBaseDto> PostAsync(OpenAiPostRequestDto dto)
    {
        _logger.LogInformation("OpenAiController.PostAsync");

        if (!ModelState.IsValid)
            return new ResponseBaseDto
            {
                Message = "Invalid request",
                Succeeded = false
            };

        try
        {
            var textContent = await _openAiRequest.CreateChatAsync(dto.Prompt);
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
        catch (Exception e)
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

        try
        {
            var textContent = await _openAiRequest.CreateChatAsync("富士山の高さは何メートルですか？");
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
        catch (Exception e)
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
