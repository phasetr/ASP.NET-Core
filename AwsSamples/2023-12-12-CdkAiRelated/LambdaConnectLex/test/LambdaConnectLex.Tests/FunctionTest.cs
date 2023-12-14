using Amazon.Lambda.LexV2Events;
using Amazon.Lambda.TestUtilities;
using Common;
using NSubstitute;
using Xunit;

namespace LambdaConnectLex.Tests;

public class FunctionTest
{
    [Fact]
    public async Task TestToUpperFunction()
    {
        var mockOpenAiRequest = Substitute.For<IOpenAiRequest>();
        mockOpenAiRequest.CreateChatAsync("hello world")
            .Returns("HELLO WORLD");
        // Invoke the lambda function and confirm the string was upper cased.
        var function = new Function(mockOpenAiRequest);
        var context = new TestLambdaContext();
        var lexV2Event = new LexV2Event
        {
            SessionState = new LexV2SessionState
            {
                Intent = new LexV2Intent
                {
                    Name = "bedrock",
                    ConfirmationState = "Qualified",
                    Slots = new Dictionary<string, LexV2Slot>
                    {
                        {
                            "freeinput", new LexV2Slot
                            {
                                Value = new LexV2SlotValue
                                {
                                    InterpretedValue = "hello world",
                                    OriginalValue = "hello world"
                                }
                            }
                        }
                    }
                }
            },
            InputTranscript = "hello world"
        };
        var returnValue = await function.FunctionHandler(lexV2Event, context);
        Assert.Equal("それでは回答します。HELLO WORLD。以上が回答です。回答に納得されていれば「はい」とお伝えください。納得されていなければ「いいえ」とお伝えください。", returnValue);
    }
}
