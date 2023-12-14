using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Common;
using Microsoft.Extensions.DependencyInjection;
using OpenAI_API;

namespace LambdaConnectLex;

public static class Startup
{
    private static IServiceProvider? _serviceProvider;
    public static IServiceProvider ServiceProvider => _serviceProvider ??= InitializeServiceProvider();

    // ReSharper disable once MemberCanBePrivate.Global
    public static void AddDefaultServices(IServiceCollection services)
    {
        // パラメータストアからAPIキーを取得
        var request = new GetParameterRequest
        {
            Name = "OPENAI_API_KEY",
            WithDecryption = true
        };
        var client = new AmazonSimpleSystemsManagementClient();
        var parameterResponse = client.GetParameterAsync(request).Result;
        var apiKey = parameterResponse.Parameter.Value;

        // DI
        services.AddScoped<IOpenAiRequest, OpenAiRequest>(_ =>
        {
            var api = new OpenAIAPI(apiKey);
            return new OpenAiRequest(api);
        });
    }

    private static IServiceProvider InitializeServiceProvider()
    {
        var services = new ServiceCollection();
        AddDefaultServices(services);
        return services.BuildServiceProvider();
    }
}
