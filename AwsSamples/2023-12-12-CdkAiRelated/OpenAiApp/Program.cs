using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Common;
using Microsoft.Extensions.DependencyInjection;
using OpenAI_API;

try
{
    // パラメータストアからAPIキーを取得
    var request = new GetParameterRequest
    {
        Name = "OPENAI_API_KEY",
        WithDecryption = true
    };
    var client = new AmazonSimpleSystemsManagementClient();
    var response = await client.GetParameterAsync(request);
    var apiKey = response.Parameter.Value;

    // DI
    var serviceCollection = new ServiceCollection();
    serviceCollection.AddScoped<IOpenAiRequest, OpenAiRequest>(_ =>
    {
        var api = new OpenAIAPI(apiKey);
        return new OpenAiRequest(api);
    });
    var serviceProvider = serviceCollection.BuildServiceProvider();
    var openAiRequest = serviceProvider.GetService<IOpenAiRequest>();
    if (openAiRequest == null)
    {
        Console.WriteLine("openAiRequest is null");
        return;
    }

    var result = await openAiRequest.CreateChatAsync("富士山の高さは何メートルですか？");
    Console.WriteLine(result);
}
catch (AmazonSimpleSystemsManagementException e)
{
    Console.WriteLine(e.Message);
    Console.WriteLine(e.StackTrace);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    Console.WriteLine(e.StackTrace);
}
