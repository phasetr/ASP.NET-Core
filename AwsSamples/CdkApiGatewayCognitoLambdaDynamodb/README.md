# Fine-grained access control for API Gateway using Lambda Authorizer

- Original: [Fine-grained access control for API Gateway using Lambda Authorizer](https://github.com/aws-samples/aws-cdk-examples/tree/master/csharp/apigateway-cognito-lambda-dynamodb)
- Blazor GitHub: [Sample Blazor Webassembly project Using Cognito Hosted UI Authentication](https://github.com/sravimohan/blazor-webassembly-cognito-hosted-ui-sample)
- `TODO`：ローカルで立ち上げた`Blazor`のフロントエンドからサーバー上のAPIへのアクセスで`CORS`を通す
- `APIGateway`の`URL`取得

```shell
aws cloudformation describe-stacks --stack-name ApiGatewayAuthStack --query 'Stacks[].Outputs[?OutputKey==`ApiGwEndpoint`].OutputValue' --output text
```

## How it works

Users must present a JWT token from Cognito in the request to API Gateway. The AWS Cognito hosted UI, set up via the
CDK, is the simplest way to interact with Cognito. If the header is not present in the request, API Gateway will return
an HTTP401 Unauthorized status code. If a token is present in the request header then the request is passed on to the
authentication lambda function for validation.

The token signature gets verified by the Auth lambda using JSON Web Keys (JWKs) provided by a Cognito User pool.

Once the token signature is verified (both in structure and expiry), code verifies the token's claims and retrieves the
user group associated with the token. Based on that user group, the Lambda function reads the API Gateway access policy
document from the DynamoDB table. If user group not present in DynamoDB table then the function returns a deny policy
which will make API Gateway return an HTTP 403 Forbidden response to user. If user group is present in the DynamoDB
table, the associated policy document will be return to API Gateway.

Based on the policy returned by Auth Lambda function, API Gateway decides either to forward the request to backend
Lambda or return an HTTP 403 Forbidden response. If JWT token is invalid, in terms of JWT structure or signature, the
authentication function raises an "Unauthorized" exception which in turns into 401 Unauthorized response back to the
user.

As populated in DynamoDB as part of deployment steps, this configuration creates two user groups: `read-only`
and `read-update-add`. `read-only` may only call GET on the backend, while `read-update-add` may make GET and POST
operations against the backend.

## Deploy

```shell
rm -rf dist
dotnet build
dotnet publish src/Lambda/BackendFunction/BackendFunction.csproj -c Release -o dist/BackendFunction
dotnet publish src/Lambda/AuthFunction/AuthFunction.csproj -c Release -o dist/AuthFunction
cdk deploy ApiGatewayAuthStack --app 'dotnet run --project src/CDK/cdk.csproj'
```

- `DynamoDB`に初期値を登録

```shell
aws dynamodb batch-write-item --request-items file://src/DynamoDBData.json
```

## Testing

1. AWSコンソールでCognitoにアクセスする（コンソールでの操作は面倒なため`AWS CLI`での実行法を追記している）。
2. `CognitoUserPool`でユーザーを作る。メールアドレスとパスワードは覚えておこう。
   ![CreateUser](CognitoUserCreate.png)

```shell
aws cognito-idp list-user-pools --max-results 1 | jq ".UserPools[] | {Id, Name}"
aws cognito-idp list-user-pools --max-results 1 --query 'UserPools[]' --output text
export CognitoId=$(aws cognito-idp list-user-pools --max-results 1 --query 'UserPools[].Id' --output text)
export CognitoUserPoolName=$(aws cognito-idp list-user-pools --max-results 1 --query 'UserPools[].Name' --output text)
```

- ユーザー作成

```shell
export UserName="phasetr@gmail.com"
aws cognito-idp admin-create-user \
  --user-pool-id ${CognitoId} \
  --username ${UserName} \
  --user-attributes Name=email,Value="${UserName}" Name=email_verified,Value=true \
  --message-action SUPPRESS
```

- パスワード設定

```shell
aws cognito-idp admin-set-user-password \
  --user-pool-id ${CognitoId} \
  --username ${UserName} \
  --password 'P@ssw0rd' \
  --permanent
```

- ユーザの情報確認

```shell
aws cognito-idp admin-get-user \
  --user-pool-id ${CognitoId} \
  --username ${UserName}
```

3. `read-only`のユーザーグループに新規ユーザーを追加する。
   ![AssignUserToUserGroup](AssignUserToGroup.png)

```shell
aws cognito-idp admin-add-user-to-group \
  --user-pool-id ${CognitoId} \
  --username ${UserName} \
  --group-name "USERPOOLGROUP#read-only"
```

- グループ確認

```shell
aws cognito-idp list-users-in-group \
  --user-pool-id ${CognitoId} \
  --group-name "USERPOOLGROUP#read-only"
```

4. ユーザーを追加し、ユーザーグループ`read-update-add`を追加する。

- ユーザー作成

```shell
export UserName="phasetr+admin@gmail.com"
aws cognito-idp admin-create-user \
  --user-pool-id ${CognitoId} \
  --username ${UserName} \
  --user-attributes Name=email,Value="${UserName}" Name=email_verified,Value=true \
  --message-action SUPPRESS
```

- パスワード設定

```shell
aws cognito-idp admin-set-user-password \
  --user-pool-id ${CognitoId} \
  --username ${UserName} \
  --password 'P@ssw0rd' \
  --permanent
```

- ユーザの情報確認

```shell
aws cognito-idp admin-get-user \
  --user-pool-id ${CognitoId} \
  --username ${UserName}
```

```shell
aws cognito-idp admin-add-user-to-group \
  --user-pool-id ${CognitoId} \
  --username ${UserName} \
  --group-name "USERPOOLGROUP#read-update-add"
```

- グループ確認

```shell
aws cognito-idp list-users-in-group \
  --user-pool-id ${CognitoId} \
  --group-name "USERPOOLGROUP#read-update-add"
```

5. 次のコマンドで`CognitoHostedUIUrl`を確認して、
   `CDK`で作った`CognitoHostedUIUrl`の`Cognito`アプリクライアントにアクセスする。 

```shell
aws cloudformation describe-stacks --stack-name ApiGatewayAuthStack --query 'Stacks[].Outputs[?OutputKey==`CognitoHostedUIUrl`].OutputValue' --output text
```

6. `read-only`ユーザーグループに割り当てたユーザーでログインする。
7. 上記手順で遷移した`URL`から`access_token`を取得する。以下のコマンドはコールバック`URL`から取得した`access_token`を貼り付けている。

```shell
export AccessToken="<ログイン後URLから取得>"
```

8. `ApiGwEndpoint`の`URL`を取得し、`Authorization`ヘッダーに`access_token`を設定して`GET`リクエストを送信する。
   ![PostmanCall](PostmanCall.png)

```shell
export ApiGwEndpoint=$(aws cloudformation describe-stacks --stack-name ApiGatewayAuthStack --query 'Stacks[].Outputs[?OutputKey==`ApiGwEndpoint`].OutputValue' --output text)
echo ${ApiGwEndpoint}
```

```shell
curl -H "Authorization: Bearer ${AccessToken}" -i ${ApiGwEndpoint}
```

```shell
curl -X POST -H "Content-Type: application/json" -H "Authorization: Bearer ${AccessToken}" -i ${ApiGwEndpoint}
```

9. Confirm that you are returned an 200 Success response
10. Now invoke the same request with the POST verb and the same `access_token`.
11. Confirm that you are returned an HTTP 403 Unauthorized response.
12. Log into the Hosted UI as the second user you created following the steps previously described.
13. Make a GET request to the API Gateway HTTP Endpoint as you did previously, 
    this time using the second user's `access_token` for the bearer token.
14. Confirm that you are returned a 200 Success response
15. Make a POST request to the API HTTP Endpoint
16. Confirm you are returned an HTTP 201 Created response
17. Try making a GET or POST request to the endpoint with invalid token
18. Observe that you're returned an HTTP 401 Unauthorized response

## Cleanup

Run the following commands at eventbridge-firehose-s3-cdk folder level

1. Delete the stack

```bash
cdk destroy ApiGatewayAuthStack --app 'dotnet run --project src/CDK/cdk.csproj'
```

2. Confirm the stack has been deleted

```bash
aws cloudformation list-stacks --query "StackSummaries[?contains(StackName,'ApiGatewayAuthStack')].StackStatus"
```

## Related resources

- [Verifying a JSON web token](https://docs.aws.amazon.com/cognito/latest/developerguide/amazon-cognito-user-pools-using-tokens-verifying-a-jwt.html)
- [API GW Resource Policies](https://docs.aws.amazon.com/apigateway/latest/developerguide/apigateway-resource-policies.html)
- [Control access for invoking an API](https://docs.aws.amazon.com/apigateway/latest/developerguide/api-gateway-control-access-using-iam-policies-to-invoke-api.html)
- [API GW Policy statement resource expression format](https://docs.aws.amazon.com/apigateway/latest/developerguide/api-gateway-control-access-using-iam-policies-to-invoke-api.html#api-gateway-iam-policy-resource-format-for-executing-api)
- [Lambda authorizer request format](https://docs.aws.amazon.com/apigateway/latest/developerguide/api-gateway-lambda-authorizer-input.html)
- [Lambda authorizer response format](https://docs.aws.amazon.com/apigateway/latest/developerguide/api-gateway-lambda-authorizer-output.html)
- [Amazon Cognito hosted UI](https://docs.aws.amazon.com/cognito/latest/developerguide/cognito-user-pools-app-integration.html)
- [API Gateway Lambda authorizers](https://docs.aws.amazon.com/apigateway/latest/developerguide/apigateway-use-lambda-authorizer.html)
