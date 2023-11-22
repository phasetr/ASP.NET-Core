#!/bin/bash
rm -rf dist
dotnet build
dotnet publish src/Lambda/BackendFunction/BackendFunction.csproj -c Release -o dist/BackendFunction
dotnet publish src/Lambda/AuthFunction/AuthFunction.csproj -c Release -o dist/AuthFunction
cdk deploy ApiGatewayAuthStack --app 'dotnet run --project src/CDK/cdk.csproj'

export ApiGwEndpoint=$(aws cloudformation describe-stacks \
  --stack-name ApiGatewayAuthStack \
  --query 'Stacks[].Outputs[?OutputKey==`ApiGwEndpoint`].OutputValue' --output text)
export CognitoHostedUIUrl=$(aws cloudformation describe-stacks \
  --stack-name ApiGatewayAuthStack \
  --query 'Stacks[].Outputs[?OutputKey==`CognitoHostedUIUrl`].OutputValue' --output text)
  
blazor=$(cat <<EOS
{
  "ApiBaseAddress": "${ApiGwEndpoint}",
  "CognitoHostedUiUrl": "${CognitoHostedUIUrl}"
}
EOS)
echo "$blazor" > Blazor/wwwroot/appsettings.json  

cat <<EOS
初回デプロイ時はREADMEを見てCognitoのユーザープールを設定すること。
EOS