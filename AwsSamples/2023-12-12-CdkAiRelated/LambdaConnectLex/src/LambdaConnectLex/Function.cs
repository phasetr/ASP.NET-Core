using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.LexV2Events;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Common;
using Microsoft.Extensions.DependencyInjection;
using OpenAI_API;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace LambdaConnectLex;

public class Function
{
    private IOpenAiRequest _openAiRequest;

    public Function(IOpenAiRequest openAiRequest)
    {
        _openAiRequest = openAiRequest;
    }

    /// <summary>
    ///     A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="lexV2Event"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<string> FunctionHandler(LexV2Event lexV2Event, ILambdaContext context)
    {
        // パラメータストアからAPIキーを取得
        var request = new GetParameterRequest
        {
            Name = "OPENAI_API_KEY",
            WithDecryption = true
        };
        var client = new AmazonSimpleSystemsManagementClient();
        var parameterResponse = await client.GetParameterAsync(request);
        var apiKey = parameterResponse.Parameter.Value;

        // DI
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<IOpenAiRequest, OpenAiRequest>(_ =>
        {
            var api = new OpenAIAPI(apiKey);
            return new OpenAiRequest(api);
        });
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _openAiRequest = serviceProvider.GetService<IOpenAiRequest>() ?? throw new InvalidOperationException();

        Console.WriteLine("Received LexV2Event: " + JsonSerializer.Serialize(lexV2Event));
        var intentName = lexV2Event.SessionState.Intent.Name;
        if (intentName != "bedrock") return "お力になれず申し訳ありません。電話を切ります。";

        var response = await OpenAiIntentAsync(lexV2Event);
        return response.Messages[0].Content;
    }

    private static LexV2Response ElicitSlot(
        string slotToElicit, string intentName,
        IDictionary<string, LexV2Slot> slots)
    {
        return new LexV2Response
        {
            SessionState = new LexV2SessionState
            {
                DialogAction = new LexV2DialogAction
                {
                    Type = "ElicitSlot",
                    SlotToElicit = slotToElicit
                },
                Intent = new LexV2Intent
                {
                    Name = intentName,
                    Slots = slots,
                    State = "InProgress"
                }
            },
            Messages = new List<LexV2Message>
            {
                new()
                {
                    ContentType = "PlainText",
                    Content = string.Empty
                }
            }
        };
    }

    private static LexV2Response ConfirmIntent(string messageContent, string intentName,
        IDictionary<string, LexV2Slot> slots)
    {
        return new LexV2Response
        {
            SessionState = new LexV2SessionState
            {
                DialogAction = new LexV2DialogAction
                {
                    Type = "ConfirmIntent"
                },
                Intent = new LexV2Intent
                {
                    Name = intentName,
                    Slots = slots,
                    State = "Fulfilled"
                }
            },
            Messages = new List<LexV2Message>
            {
                new()
                {
                    ContentType = "PlainText",
                    Content = messageContent
                }
            }
        };
    }

    private static LexV2Response Close(
        string fulfilmentState, string messageContent, string intentName,
        IDictionary<string, LexV2Slot> slots)
    {
        return new LexV2Response
        {
            SessionState = new LexV2SessionState
            {
                DialogAction = new LexV2DialogAction
                {
                    Type = "Close"
                },
                Intent = new LexV2Intent
                {
                    Name = intentName,
                    Slots = slots,
                    State = fulfilmentState
                }
            },
            Messages = new List<LexV2Message>
            {
                new()
                {
                    ContentType = "PlainText",
                    Content = messageContent
                }
            }
        };
    }

    private async Task<LexV2Response> OpenAiIntentAsync(LexV2Event lexV2Event)
    {
        Console.WriteLine("Received LexV2Event: " + JsonSerializer.Serialize(lexV2Event));
        var intentName = lexV2Event.SessionState.Intent.Name;
        var slots = lexV2Event.SessionState.Intent.Slots;
        var inputText = lexV2Event.InputTranscript;

        if (slots["freeinput"].Value == null) return ElicitSlot("freeinput", intentName, slots);

        var confirmationStatus = lexV2Event.SessionState.Intent.ConfirmationState;
        switch (confirmationStatus)
        {
            case "Confirmed":
                return Close("Fulfilled", "それでは電話を切ります。", intentName, slots);
            case "Denied":
                return Close("Failed", "お力になれず申し訳ありません。電話を切ります。", intentName, slots);
        }

        var responseText = await _openAiRequest.CreateChatAsync(inputText);
        Console.WriteLine("ResponseText: " + responseText);
        return ConfirmIntent(
            $"それでは回答します。{responseText}。以上が回答です。回答に納得されていれば「はい」とお伝えください。納得されていなければ「いいえ」とお伝えください。",
            intentName,
            slots);
    }
}
