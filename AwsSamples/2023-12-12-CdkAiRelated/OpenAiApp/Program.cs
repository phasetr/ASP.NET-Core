using Amazon.SimpleSystemsManagement;
using Common;
using Microsoft.Extensions.DependencyInjection;

try
{
    // DI
    var serviceCollection = new ServiceCollection();
    serviceCollection.AddSingleton<IAmazonSimpleSystemsManagement, AmazonSimpleSystemsManagementClient>();
    serviceCollection.AddTransient<IOpenAiRequest, OpenAiRequest>();
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
