using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.CodeBuild;
using Amazon.CDK.AWS.CodeCommit;
using Amazon.CDK.Pipelines;
using Constructs;

namespace Workshop;

public class WorkshopPipelineStack : Stack
{
    public WorkshopPipelineStack(Construct parent, string id, IStackProps props = null) : base(parent, id, props)
    {
        var repo = new Repository(this, "WorkshopRepo", new RepositoryProps
        {
            RepositoryName = "WorkshopRepo"
        });

        var pipeline = new CodePipeline(this, "Pipeline", new CodePipelineProps
        {
            CodeBuildDefaults = new CodeBuildOptions
            {
                BuildEnvironment = new BuildEnvironment
                {
                    BuildImage = LinuxBuildImage.STANDARD_7_0
                }
            },
            PipelineName = "WorkshopPipeline",
            Synth = new ShellStep("Synth", new ShellStepProps
            {
                Input = CodePipelineSource.CodeCommit(repo, "main"),
                Commands = new[]
                {
                    "npm install -g aws-cdk",
                    "cd src/Workshop && dotnet build",
                    "cd ../../",
                    "npx cdk synth"
                }
            })
        });
        var deploy = new PipelineStage(this, "Deploy");
        var deployStage = pipeline.AddStage(deploy);

        deployStage.AddPost(new ShellStep("TestViewerEndpoint", new ShellStepProps
        {
            EnvFromCfnOutputs = new Dictionary<string, CfnOutput>
            {
                {"ENDPOINT_URL", deploy.HCViewerUrl}
            },
            Commands = new[] {"curl -Ssf $ENDPOINT_URL"}
        }));
        deployStage.AddPost(new ShellStep("TestAPIGatewayEndpoint", new ShellStepProps
        {
            EnvFromCfnOutputs = new Dictionary<string, CfnOutput>
            {
                {"ENDPOINT_URL", deploy.HCEndpoint}
            },
            Commands = new[]
            {
                "curl -Ssf $ENDPOINT_URL/",
                "curl -Ssf $ENDPOINT_URL/hello",
                "curl -Ssf $ENDPOINT_URL/test"
            }
        }));
    }
}
