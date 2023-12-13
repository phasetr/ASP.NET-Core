using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using OpenAI_API;
using OpenAI_API.Chat;

namespace Common;

public class OpenAiRequest : IOpenAiRequest
{
    private readonly IAmazonSimpleSystemsManagement _client;

    public OpenAiRequest(IAmazonSimpleSystemsManagement client)
    {
        _client = client;
    }

    public async Task<string> CreateChatAsync(string textContent)
    {
        var request = new GetParameterRequest
        {
            Name = "OPENAI_API_KEY",
            WithDecryption = true
        };
        var response = await _client.GetParameterAsync(request);
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
        return result.Choices[0].Message.TextContent;
    }
}
