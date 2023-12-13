using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using OpenAI_API;
using OpenAI_API.Chat;

var client = new AmazonSimpleSystemsManagementClient();
var request = new GetParameterRequest
{
    Name = "OPENAI_API_KEY",
    WithDecryption = true
};

try
{
    // var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
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
    Console.WriteLine(result.Choices[0].Message.TextContent);

    // var result = await api.Completions.GetCompletion("楕円型非線型偏微分方程式の応用に関して教えてください。");
    // Console.WriteLine(result.Trim());
}
catch (AmazonSimpleSystemsManagementException e)
{
    Console.WriteLine(e.Message);
    Console.WriteLine(e.StackTrace);
}
