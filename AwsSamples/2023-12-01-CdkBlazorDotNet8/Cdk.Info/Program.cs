using Amazon.CloudFormation;
using Amazon.CloudFormation.Model;
using Cdk.Common;

const string stackName = $"{Constants.StackName}-dev";
var client = new AmazonCloudFormationClient();
var request = new DescribeStacksRequest {StackName = stackName};
var response = await client.DescribeStacksAsync(request);
var outputs = response.Stacks[0].Outputs;
foreach (var output in outputs)
{
    Console.WriteLine($"{output.OutputKey} = {output.OutputValue}");
}
