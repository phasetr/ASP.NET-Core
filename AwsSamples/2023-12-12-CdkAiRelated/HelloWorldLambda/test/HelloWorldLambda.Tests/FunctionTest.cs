using Amazon.Lambda.TestUtilities;
using Xunit;

namespace HelloWorldLambda.Tests;

public class FunctionTest
{
    [Fact]
    public void TestToUpperFunction()
    {
        // Invoke the lambda function and confirm the string was upper cased.
        var function = new Function();
        var context = new TestLambdaContext();
        var payload = new Payload
        {
            Input = "hello world"
        };
        var upperCase = function.FunctionHandler(payload, context);

        Assert.Equal("HELLO WORLD", upperCase);
    }
}
