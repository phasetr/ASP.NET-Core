using OpenAI_API;
using OpenAI_API.Chat;

namespace Common;

public class OpenAiRequest : IOpenAiRequest
{
    private readonly IOpenAIAPI _api;

    public OpenAiRequest(IOpenAIAPI api)
    {
        _api = api;
    }

    public async Task<string> CreateChatAsync(string textContent)
    {
        var message = new ChatMessage
        {
            Role = ChatMessageRole.User,
            TextContent = textContent
        };
        var result = await _api.Chat.CreateChatCompletionAsync(
            model: "gpt-3.5-turbo",
            messages: new List<ChatMessage> {message});
        return result.Choices[0].Message.TextContent;
    }
}
