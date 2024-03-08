# README

- `TODO`: `2023/12/1`時点でまだ`Lambda`で`.NET8`が使えないため動作確認できたわけではない
- `BlazorDynamoDb`では,
  `AspNetCore.Identity.AmazonDynamoDB`を使って`DynamoDB`で`Identity`を使えるようにしてある
- (`Rider`の)`.NET8`での`Blazor Web App`から生成したサンプルがオリジナル
- 参考
  - [dotnet / blazor-samples](https://github.com/dotnet/blazor-samples)
  - .NET8, [BlazorWebAppEFCore](https://github.com/dotnet/blazor-samples/tree/main/8.0/BlazorWebAppEFCore)
  - .NET8, [BlazorWebAssemblyStandaloneWithIdentity](https://github.com/dotnet/blazor-samples/tree/main/8.0/BlazorWebAssemblyStandaloneWithIdentity)
  - GitHub, [BasicTestApp](https://github.com/dotnet/aspnetcore/tree/main/src/Components/test/testassets/BasicTestApp)
  - `DynamoDB`での`Identity`: [AspNetCore.Identity.AmazonDynamoDB](https://github.com/ganhammar/AspNetCore.Identity.AmazonDynamoDB)
    - 上記ライブラリ作者の趣味プロジェクト: [what-did-i-do-login](https://github.com/ganhammar/what-did-i-do-login)

## `AWS`コマンド類

```shell
aws cloudformation describe-stacks \
  --stack-name cdk-blazor-dotnet8-stack \
  --query 'Stacks[].Outputs[?OutputKey==`cdkfullstackwithauthcognitoappclientiddev`].OutputValue' \
  --output text
```

## ECR

- 先に`CDK`で`ECR`以外をコメントアウトして、先に`ECR`リポジトリだけ作っておく
- コンソールで`ECR`にアクセス
- `プッシュコマンドの表示`からコマンドを転記する
- ソリューションルートで次のコマンドを実行する

```shell
aws ecr get-login-password --region ap-northeast-1 | docker login --username AWS --password-stdin 573143736992.dkr.ecr.ap-northeast-1.amazonaws.com
docker build -t cdk-blazor-dotnet8-ecr-repository-dev . -f Blazor/Dockerfile
docker tag cdk-blazor-dotnet8-ecr-repository-dev:latest 573143736992.dkr.ecr.ap-northeast-1.amazonaws.com/cdk-blazor-dotnet8-ecr-repository-dev:latest
docker push 573143736992.dkr.ecr.ap-northeast-1.amazonaws.com/cdk-blazor-dotnet8-ecr-repository-dev:latest
```

- 次のコマンドでローカルでサンプル実行できる

```shell
docker build -t cdk-blazor-dotnet8-ecr-repository-dev . -f Blazor/Dockerfile
docker run -it --rm -p 80:8080 cdk-blazor-dotnet8-ecr-repository-dev
```

### `Api`に対する確認

```shell
aws ecr get-login-password --region ap-northeast-1 | docker login --username AWS --password-stdin 573143736992.dkr.ecr.ap-northeast-1.amazonaws.com
docker build -t cdk-blazor-dotnet8-ecr-repository-dev . -f Api/Dockerfile
docker tag cdk-blazor-dotnet8-ecr-repository-dev:latest 573143736992.dkr.ecr.ap-northeast-1.amazonaws.com/cdk-blazor-dotnet8-ecr-repository-dev:latest
docker push 573143736992.dkr.ecr.ap-northeast-1.amazonaws.com/cdk-blazor-dotnet8-ecr-repository-dev:latest
```

```shell
docker build -t cdk-blazor-dotnet8-ecr-repository-dev . -f Api/Dockerfile
docker run -it --rm -p 80:8080 cdk-blazor-dotnet8-ecr-repository-dev
```
