#!/bin/bash
stackName="ba-dev"
frontendDirectory="BlazorWasmAuth"
backendDirectory="Backend"

cat <<EOS
cdk deploy：特にバックエンドのデプロイ
EOS

cdk deploy --profile dev --require-approval never

cat <<EOS
フロントエンド用のlaunchSettings.jsonの設定
EOS

frontendLaunchSettingJson=$(cat <<EOS
{
  "\$schema": "http://json.schemastore.org/launchsettings.json",
  "profiles": {
    "https": {
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

echo "${frontendLaunchSettingJson}" > ${frontendDirectory}/Properties/launchSettings.json

export BackendUrl=$(aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`babagurldev`].OutputValue' --output text)
export FrontendUrl=$(aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`bafdndev`].OutputValue' \
  --output text)

frontendAppSettingsJson=$(cat <<EOS
{
  "FrontendUrl": "${FrontendUrl}",
  "BackendUrl": "${BackendUrl}"
}
EOS)
echo "$frontendAppSettingsJson" > ${frontendDirectory}/wwwroot/appsettings.json

frontendAppSettingsDevelopmentJson=$(cat <<EOS
{
  "FrontendUrl": "https://localhost:6500",
  "BackendUrl": "https://localhost:5500"
}
EOS)
echo "$frontendAppSettingsDevelopmentJson" > ${frontendDirectory}/wwwroot/appsettings.Development.json

cat <<EOS
バックエンド用のlaunchSettings.jsonの設定
EOS

backendLaunchSettingsJson=$(cat <<EOS
{
  "\$schema": "http://json.schemastore.org/launchsettings.json",
  "profiles": {
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "https://localhost:5500/swagger",
      "applicationUrl": "https://localhost:5500;http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "CLIENT_URL": "https://localhost:6500",
        "REGION": "ap-northeast-1"
      }
    }
  }
}
EOS)

echo "$backendLaunchSettingsJson" > ${backendDirectory}/Properties/launchSettings.json

cat <<EOS
フロントエンドのデプロイ
EOS

export S3BucketName=$(aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`bafbndev`].OutputValue' \
  --output text --profile dev)
echo S3 bucket name: ${S3BucketName}
dotnet publish ${frontendDirectory} -c Release -o ./publish
aws s3 sync ./publish/wwwroot s3://${S3BucketName} --profile dev

cat <<EOS
APIの実行確認
EOS

echo APIGateway URL: ${BackendUrl}
curl -s ${BackendUrl}

export DomainName=$(aws cloudformation describe-stacks --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`bafdndev`].OutputValue' --output text --profile dev)
echo "フロントエンドのURL": ${DomainName}
