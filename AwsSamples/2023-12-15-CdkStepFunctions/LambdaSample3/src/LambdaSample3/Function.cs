using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace LambdaSample3;

public class Payload
{
    [JsonPropertyName("throw")]
    public string Throw { get; set; } = string.Empty;
}

public class MyResponse
{
    public string Bar { get; set; } = default!;
}

/// <summary>
///     オリジナルは`Cdk/lambda.py`
/// </summary>
public class Function
{
    /// <summary>
    ///     A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="payload"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public MyResponse FunctionHandler(Payload payload, ILambdaContext context)
    {
        try
        {
            var random = new Random();
            var rand = random.Next(1, 4);
            Console.WriteLine($"rand: {rand}");
            var input = int.Parse(payload.Throw);
            Console.WriteLine($"input: {input}");

            string result;
            if (input == rand) result = "Tied";
            else if (input == 3 && rand == 1) result = "You win";
            else if (input == 1 && rand == 3) result = "You lose";
            else if (input < rand) result = "You win";
            else if (input > rand) result = "You lose";
            else result = "Error";
            return new MyResponse
            {
                Bar = result
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            return new MyResponse
            {
                Bar = "Exception"
            };
        }
    }
}
