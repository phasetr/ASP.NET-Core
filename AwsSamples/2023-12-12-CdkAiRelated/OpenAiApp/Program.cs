using OpenAI_API;
using OpenAI_API.Chat;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
var api = new OpenAIAPI(apiKey);
var message = new ChatMessage
{
    Role = ChatMessageRole.User,
    TextContent = "富士山の高さは何メートルですか？"
};
var result = await api.Chat.CreateChatCompletionAsync(
    model: "gpt-3.5-turbo",
    messages: new List<ChatMessage> {message});
Console.WriteLine(result.Choices[0].Message.TextContent);

// var result = await api.Completions.GetCompletion("楕円型非線型偏微分方程式の応用に関して教えてください。");
// Console.WriteLine(result.Trim());
