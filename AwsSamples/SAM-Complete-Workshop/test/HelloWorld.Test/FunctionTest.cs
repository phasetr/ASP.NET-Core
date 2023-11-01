using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using Xunit;
using Xunit.Abstractions;

namespace HelloWorld.Tests;

public class FunctionTest
{
    private static readonly HttpClient Client = new();
    private readonly ITestOutputHelper _testOutputHelper;

    public FunctionTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private static async Task<string> GetCallingIp()
    {
        Client.DefaultRequestHeaders.Accept.Clear();
        Client.DefaultRequestHeaders.Add("User-Agent", "AWS Lambda .Net Client");

        var stringTask = Client.GetStringAsync("https://checkip.amazonaws.com/").ConfigureAwait(false);

        var msg = await stringTask;
        return msg.Replace("\n", "");
    }

    [Fact]
    public async Task TestHelloWorldFunctionHandler()
    {
        var request = new APIGatewayProxyRequest();
        var context = new TestLambdaContext();
        var location = GetCallingIp().Result;
        var body = new Dictionary<string, string>
        {
            {"message", "hello my friend"},
            {"location", location}
        };

        var expectedResponse = new APIGatewayProxyResponse
        {
            Body = JsonSerializer.Serialize(body),
            StatusCode = 200,
            Headers = new Dictionary<string, string> {{"Content-Type", "application/json"}}
        };

        var function = new Function();
        var response = await function.FunctionHandler(request, context);

        _testOutputHelper.WriteLine("Lambda Response: \n" + response.Body);
        _testOutputHelper.WriteLine("Expected Response: \n" + expectedResponse.Body);

        Assert.Equal(expectedResponse.Body, response.Body);
        Assert.Equal(expectedResponse.Headers, response.Headers);
        Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
    }
}
