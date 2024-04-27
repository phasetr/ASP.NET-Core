#!/bin/bash
stackName="ba-dev"
frontendDirectory="BlazorWasmAuth"
backendDirectory="Api"

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
EOS
)

echo "${frontendLaunchSettingJson}" > ${frontendDirectory}/Properties/launchSettings.json

BackendUrl=$(aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`babagurldev`].OutputValue' --output text)
FrontendUrl=$(aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`bafdndev`].OutputValue' \
  --output text)

frontendAppSettingsJson=$(cat <<EOS
{
  "Url": {
    "FrontendUrl": "${FrontendUrl}",
    "BackendUrl": "${BackendUrl}"
  }
}
EOS
)
echo "$frontendAppSettingsJson" > ${frontendDirectory}/wwwroot/appsettings.json

frontendAppSettingsDevelopmentJson=$(cat <<EOS
{
  "Url": {
    "FrontendUrl": "https://localhost:6500",
    "BackendUrl": "https://localhost:5500"
  }
}
EOS
)
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
        "AWS_REGION": "ap-northeast-1",
        "TABLE_NAME": "ba-ddb-local"
      }
    }
  }
}
EOS
)

echo "$backendLaunchSettingsJson" > ${backendDirectory}/Properties/launchSettings.json

devDeletedBackendUrl="$(echo ${BackendUrl} | rev | cut -c $((5+1))- | rev)"
ddbTableName=$(aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`baddbtndev`].OutputValue' \
  --output text --profile dev)
backendAppSettingsJson=$(cat <<EOS
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Url": {
    "FrontendUrl": "${FrontendUrl}",
    "BackendUrl": "${devDeletedBackendUrl}"
  },
  "DynamoDbSettings": {
    "TableName": "${ddbTableName}"
  }
}
EOS
)
echo "$backendAppSettingsJson" > ${backendDirectory}/appsettings.json

backendAppSettingsDevelopmentJson=$(cat <<EOS
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Url": {
    "FrontendUrl": "https://localhost:6500",
    "BackendUrl": "https://localhost:5500"
  },
  "DynamoDbSettings": {
    "TableName": "ba-ddb-local"
  }
}
EOS
)
echo "$backendAppSettingsDevelopmentJson" > ${backendDirectory}/appsettings.Development.json

cat <<EOS


フロントエンドのデプロイ


EOS

S3BucketName=$(aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`bafbndev`].OutputValue' \
  --output text --profile dev)
echo S3 bucket name: ${S3BucketName}
dotnet publish ${frontendDirectory} -c Release -o ./publish
aws s3 sync ./publish/wwwroot s3://${S3BucketName} --profile dev --cache-control "max-age=0, no-cache, no-store, must-revalidate"
rm -rf ./publish

cat <<EOS

CloudFrontのキャッシュの削除

EOS

DistributionId=$(aws cloudformation describe-stacks \
  --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`bafcfdiddev`].OutputValue' \
  --output text --profile dev)
aws cloudfront create-invalidation --distribution-id ${DistributionId} --paths '/*' --output text --profile dev

cat <<EOS


APIの実行確認


EOS

echo APIGateway URL: ${BackendUrl}
curl -s ${BackendUrl}

DomainName=$(aws cloudformation describe-stacks --stack-name ${stackName} \
  --query 'Stacks[].Outputs[?OutputKey==`bafdndev`].OutputValue' --output text --profile dev)
echo "フロントエンドのURL": ${DomainName}
