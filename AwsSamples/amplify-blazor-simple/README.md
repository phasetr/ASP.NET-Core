# Amplify-Blazor

- CI/CDがあるため、実際に動かす場合はこのディレクトリを別途切り出してリポジトリを作ること
- [Deploy .NET Blazor WebAssembly Application to AWS Amplify](https://aws.amazon.com/jp/blogs/devops/deploy-net-blazor-webassembly-application-to-aws-amplify/)
  - 記事中の`amplify.yml`は間違っていて、コミットされた`amplify.yml`は正しい形にしている

## memo

- Blazorのアプリを作って起動まで確認する

```shell
dotnet new blazorwasm
dotnet run
```

- アプリを落とす

```shell
dotnet new gitignore
```

- `GitHub`にリポジトリを作って`main`ブランチにpush
- AWSコンソールから`Amplify`を開く：適切に設定する
