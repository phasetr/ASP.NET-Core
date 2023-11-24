#!/bin/bash
stackName="cdk-fullstack-with-auth-stack-dev"

cat <<EOS
cdk deploy：特にバックエンドのデプロイ
EOS

cdk deploy --profile dev

cat <<EOS
フロントエンド用のlaunchSettings.jsonの設定
EOS

blazor=$(cat <<EOS
{
  "profiles": {
    "Blazor": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "inspectUri": "{wsProtocol}://{url.hostname}:{url.port}/_framework/debug/ws-proxy?browser={browserInspectUri}",
      "applicationUrl": "https://localhost:6500;http://localhost:6000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
EOS)

echo "$blazor" > Blazor/Properties/launchSettings.json

export ApiGwEndpoint=$(aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`cdkfullstackwithauthapigwurldev`].OutputValue' --output text)
export CognitoHostedUiRootUrl=$(aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`cdkfullstackwithauthcognitohosteduiurlroot`].OutputValue' --output text)
export CloudFrontDomainName=$(aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`cdkfullstackwithauthcloudfrontdomainnamedev`].OutputValue' \
  --output text)
encodedCloudFrontDomainName=$(echo ${CloudFrontDomainName} | jq -Rr '@uri')
encodedLocalHost=$(echo "https://localhost:6500" | jq -Rr '@uri')

blazor=$(cat <<EOS
{
  "ApiBaseAddress": "${ApiGwEndpoint}",
  "CognitoHostedUiUrl": "${CognitoHostedUiRootUrl}${encodedCloudFrontDomainName}"
}
EOS)
echo "$blazor" > Blazor/wwwroot/appsettings.json

blazor=$(cat <<EOS
{
  "ApiBaseAddress": "https://localhost:5500",
  "CognitoHostedUiUrl": "${CognitoHostedUiRootUrl}${encodedLocalHost}"
}
EOS)
echo "$blazor" > Blazor/wwwroot/appsettings.Development.json

cat <<EOS
バックエンド用のlaunchSettings.jsonの設定
EOS

export CognitoUserPoolId=$(aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`cdkfullstackwithauthcognitouserpooliddev`].OutputValue' \
  --output text)
export CognitoAppClientIdDev=$(aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`cdkfullstackwithauthcognitoappclientiddev`].OutputValue' \
  --output text)

api=$(cat <<EOS
{
  "\$schema": "http://json.schemastore.org/launchsettings.json",
  "profiles": {
    "Api": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "https://localhost:5500/swagger",
      "applicationUrl": "https://localhost:5500;http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "CLIENT_URL": "https://localhost:6500",
        "REGION": "ap-northeast-1",
        "COGNITO_USER_POOL_ID": "${CognitoUserPoolId}",
        "CLIENT_ID": "${CognitoAppClientIdDev}"
      }
    }
  }
}
EOS)

echo "$api" > Api/Properties/launchSettings.json

cat <<EOS
フロントエンドのデプロイ
EOS

export S3BucketName=$(aws cloudformation describe-stacks \
  --stack-name cdk-fullstack-with-auth-stack-dev \
  --query 'Stacks[].Outputs[?OutputKey==`cdkfullstackwithauths3bucketnamedev`].OutputValue' \
  --output text --profile dev)
echo S3 bucket name: ${S3BucketName}
dotnet publish Blazor -c Release -o ./publish
aws s3 sync ./publish/wwwroot s3://${S3BucketName} --profile dev

cat <<EOS
APIの実行確認
EOS

export ApiGwUrl=$(aws cloudformation describe-stacks --stack-name cdk-fullstack-with-auth-stack-dev --query 'Stacks[].Outputs[?OutputKey==`cdkfullstackwithauthapigwurldev`].OutputValue' --output text --profile dev)
echo APIGateway URL: ${ApiGwUrl}
curl -s ${ApiGwUrl}

cat <<EOS
フロントエンドのURL
EOS
export DomainName=$(aws cloudformation describe-stacks --stack-name cdk-fullstack-with-auth-stack-dev --query 'Stacks[].Outputs[?OutputKey==`cdkfullstackwithauthcloudfrontdomainnamedev`].OutputValue' --output text --profile dev)
echo domain name: ${DomainName}
