using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace BackendFunction;

public class Function
{
    /// <summary>
    ///     Lambda function handler to return response for GET and POST endpoint
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns>APIGatewayProxyResponse</returns>
    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return request.HttpMethod.ToUpper() switch
        {
            "GET" => new APIGatewayProxyResponse
            {
                StatusCode = (int) HttpStatusCode.OK,
                Body = "Hello",
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "text/plain"},
                    {"Access-Control-Allow-Origin", "https://localhost:5500"},
                    {"Access-Control-Allow-Headers", "Content-Type"},
                    {"Access-Control-Allow-Methods", "OPTIONS,POST,GET"}
                }
            },
            "POST" => new APIGatewayProxyResponse
            {
                StatusCode = (int) HttpStatusCode.Created,
                Body = "Created",
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "text/plain"},
                    {"Access-Control-Allow-Origin", "https://localhost:5500"},
                    {"Access-Control-Allow-Headers", "Content-Type"},
                    {"Access-Control-Allow-Methods", "OPTIONS,POST,GET"}
                }
            },
            _ => new APIGatewayProxyResponse
            {
                StatusCode = (int) HttpStatusCode.BadRequest,
                Body = "Invalid HttpMethod",
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "text/plain"},
                    {"Access-Control-Allow-Origin", "https://localhost:5500"},
                    {"Access-Control-Allow-Headers", "Content-Type"},
                    {"Access-Control-Allow-Methods", "OPTIONS,POST,GET"}
                }
            }
        };
    }
}