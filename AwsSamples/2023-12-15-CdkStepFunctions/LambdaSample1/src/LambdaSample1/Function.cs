using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaSample1;

public class Payload
{
    public string Input { get; set; } = string.Empty;
}

public class Function
{
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="payload"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public string FunctionHandler(Payload payload, ILambdaContext context)
    {
        context.Logger.LogInformation("Hello World!");
        return payload.Input.ToUpper();
    }
}
