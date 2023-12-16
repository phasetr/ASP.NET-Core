using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace LambdaSample3;

public class Payload
{
    public string Input { get; set; } = string.Empty;
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
    public MyResponse FunctionHandler(dynamic payload, ILambdaContext context)
    {
        try
        {
            var random = new Random();
            var rand = random.Next(1, 4);
            Console.WriteLine($"payloadのダンプ");
            // Console.WriteLine(ObjectDumper(payload));
            var jsonString = JsonSerializer.Serialize(payload);
            Console.WriteLine(jsonString);

            var input = int.Parse(payload.Input);
            Console.WriteLine($"input: {input}");

            string result;
            if (input == rand) result = "Tied";
            else if (input == 3 && rand == 1) result = "You Win";
            else if (input == 1 && rand == 3) result = "You Lose";
            else if (input < rand) result = "You Win";
            else if (input > rand) result = "You Lose";
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

    private static string ObjectDumper(object obj)
    {
        var type = obj.GetType();
        var properties = type.GetProperties();
        return string.Join(", ", properties.Select(p => $"{p.Name}: {p.GetValue(obj)}"));
    }
}
