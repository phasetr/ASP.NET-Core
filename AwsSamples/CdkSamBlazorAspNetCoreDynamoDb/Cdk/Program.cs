﻿using Amazon.CDK;
using CdkSamBlazorAspNetCoreDynamoDb;

const string stackName = "cdk-sam-blazor-aspnet-core-dynamo-db-stack";

var app = new App();
var unused1 = new CdkSamBlazorAspNetCoreDynamoDbStack(app, $"{stackName}-dev", new MyStackProps
{
    MyConfiguration = new MyConfiguration
    {
        EnvironmentName = "dev"
    }
});
app.Synth();
