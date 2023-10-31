# README

- [Building Blazor WebAssembly Applications with gRPC](https://subscription.packtpub.com/book/web-development/9781804610558)
  - [GitHub](https://github.com/PacktPublishing/Building-Blazor-WebAssembly-Applications-with-gRPC)
  - サーバー側の`CRUD`処理に関連した`AutoMapper`の実装が重要
  - クライアント側も標準的な`CRUD`のコンポーネント実装が参考になる
  - ソースジェネレーターの話もある
  - `Grpc.Core`は[obsolete](https://grpc.io/blog/grpc-csharp-future/)になるため注意する
    - `gRPC`は削除

## 初期化

```shell
dotnet new tool-manifest
dotnet tool install dotnet-ef --version 6.0.5
efm add Initialize
efd update
```

## TODO

- 2022, [Blazor in Action](https://www.manning.com/books/blazor-in-action)
  - `Auth0`による認証
- [Web Development with Blazor](https://www.packtpub.com/product/web-development-with-blazor-second-edition/9781803241494)
  - `Auth0`による認証
- `Blazor` + `AWS cognito`
  - [blazor-webassembly-cognito-hosted-ui-sample](https://github.com/sravimohan/blazor-webassembly-cognito-hosted-ui-sample/tree/main)
