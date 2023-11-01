using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace HelloWorld;

public class Function
{
    private static readonly HttpClient Client = new();

    private static async Task<string> GetCallingIp()
    {
        Client.DefaultRequestHeaders.Accept.Clear();
        Client.DefaultRequestHeaders.Add("User-Agent", "AWS Lambda .Net Client");

        var msg = await Client.GetStringAsync("https://checkip.amazonaws.com/").ConfigureAwait(false);
        return msg.Replace("\n", "");
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apiGatewayProxyRequest,
        ILambdaContext context)
    {
        try
        {
            var location = await GetCallingIp();
            var body = new Dictionary<string, string>
            {
                {"message", "hello my friend"},
                {"location", location}
            };
            Console.WriteLine("body: " + JsonSerializer.Serialize(body));

            return new APIGatewayProxyResponse
            {
                Body = JsonSerializer.Serialize(body),
                StatusCode = (int) HttpStatusCode.OK,
                Headers = new Dictionary<string, string> {{"Content-Type", "application/json"}}
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            return new APIGatewayProxyResponse
            {
                Body = JsonSerializer.Serialize("{}"),
                StatusCode = (int) HttpStatusCode.InternalServerError,
                Headers = new Dictionary<string, string> {{"Content-Type", "application/json"}}
            };
        }
    }
}
