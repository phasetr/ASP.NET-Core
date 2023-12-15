using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using Amazon.CDK.AWS.StepFunctions;
using Amazon.CDK.AWS.StepFunctions.Tasks;
using Constructs;
using AssetOptions = Amazon.CDK.AWS.S3.Assets.AssetOptions;

namespace CdkStepFunctions;

public class CdkStepFunctionsStack : Stack
{
    private const string Prefix = "cdk-step-functions";

    internal CdkStepFunctionsStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        #region "Step Functions 1"

        var helloFunction = new Function(this, $"{Prefix}-lambda1", new FunctionProps
        {
            Runtime = Runtime.DOTNET_6,
            Handler = "LambdaSample1::LambdaSample1.Function::FunctionHandler",
            Timeout = Duration.Seconds(25),
            Code = Code.FromAsset("LambdaSample1/src/LambdaSample1", new AssetOptions
            {
                Bundling = new BundlingOptions
                {
                    Image = Runtime.DOTNET_6.BundlingImage,
                    User = "root",
                    OutputType = BundlingOutput.ARCHIVED,
                    Command = new[]
                    {
                        "/bin/sh",
                        "-c",
                        " dotnet tool install -g Amazon.Lambda.Tools" +
                        " && dotnet build" +
                        " && dotnet lambda package --output-package /asset-output/function.zip"
                    }
                }
            })
        });
        var stateMachine1 = new StateMachine(this, $"{Prefix}-state-machine1", new StateMachineProps
        {
            DefinitionBody = DefinitionBody.FromChainable(new LambdaInvoke(this, $"{Prefix}-lambda-task1",
                    new LambdaInvokeProps
                    {
                        LambdaFunction = helloFunction
                    })
                .Next(new Succeed(this, $"{Prefix}-greeted-world1")))
        });

        #endregion

        #region "Step Functions 2"

        var lambda2 = new Function(this, $"{Prefix}-lambda2", new FunctionProps
        {
            Runtime = Runtime.PYTHON_3_11,
            MemorySize = 256,
            LogRetention = RetentionDays.ONE_DAY,
            Handler = "lambda.lambda_handler",
            Code = Code.FromAsset("Cdk")
        });
        // 最終ステート
        var lastState2 = new Succeed(this, $"{Prefix}-last-state2");
        var stateMachine2 = new StateMachine(this, $"{Prefix}-state-machine2", new StateMachineProps
        {
            StateMachineName = $"{Prefix}-state-machine2-name",
            DefinitionBody = DefinitionBody.FromChainable(new LambdaInvoke(this, $"{Prefix}-lambda2-first-state",
                    new LambdaInvokeProps
                    {
                        LambdaFunction = lambda2,
                        Payload = TaskInput.FromJsonPathAt("$.throw")
                    })
                .Next(new Choice(this, $"{Prefix}-choice-state2")
                    .When(Condition.StringEquals("$.bar", "Tied"),
                        new Pass(this, "Tied", new PassProps
                            {
                                Result = Result.FromString("Tied")
                            })
                            .Next(lastState2))
                    .When(Condition.StringEquals("$.bar", "You win"),
                        new Pass(this, "Won", new PassProps
                            {
                                Result = Result.FromString("Won")
                            })
                            .Next(lastState2))
                    .When(Condition.StringEquals("$.bar", "You lose"),
                        new Pass(this, "Lost", new PassProps
                            {
                                Result = Result.FromString("Lost")
                            })
                            .Next(lastState2))
                    .Otherwise(new Fail(this, $"{Prefix}-default-state2", new FailProps {Cause = "Not match"}))))
        });

        #endregion

        #region Outputs

        var unused1 = new CfnOutput(this, $"{Prefix}-state-machine1-arn", new CfnOutputProps
        {
            Value = stateMachine1.StateMachineArn
        });
        var unused2 = new CfnOutput(this, $"{Prefix}-state-machine2-arn", new CfnOutputProps
        {
            Value = stateMachine2.StateMachineArn
        });

        #endregion
    }
}
