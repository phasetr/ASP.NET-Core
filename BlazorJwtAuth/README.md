# README

- `Blazor`, `API`ともに単体テストのサンプルあり.
- 参考のため`API`の統合テストもつけた.
- `Blazor Wasm`の場合は`API`に`CORS`を設定すること.

## 参考資料

- [ORIGINAL](https://github.com/iammukeshm/JWTAuthentication.WebApi)
- [Build Secure ASP.NET Core API with JWT Authentication – Detailed Guide](https://codewithmukesh.com/blog/aspnet-core-api-with-jwt-authentication/)
- [How to Use Refresh Tokens in ASP.NET Core APIs – JWT Authentication](https://codewithmukesh.com/blog/refresh-tokens-in-aspnet-core/)
- 認証
  - [.NET 6.0 JWT Token Authentication C# API Tutorial](https://trystanwilcock.com/2022/08/13/net-6-0-jwt-token-authentication-c-sharp-api-tutorial/) 
  - [.NET 6.0 Blazor WebAssembly JWT Token Authentication From Scratch C# Tutorial](https://trystanwilcock.com/2022/09/28/net-6-0-blazor-webassembly-jwt-token-authentication-from-scratch-c-sharp-wasm-tutorial/)
- 結合テストの参考ページ: [ASP.NET Core で Web API の結合テストをしよう](https://qiita.com/okazuki/items/cbda6c456dcba8fee503) 
- 参考：Blazor向け認証, [The bullshit-less ASP.NET Blazor WASM JWT authentication tutorial from the ground up.](https://www.reddit.com/r/csharp/comments/u6n8nz/the_bullshitless_aspnet_blazor_wasm_jwt/)
- Auth0認証実装：[Securing Blazor WebAssembly Apps](https://auth0.com/blog/securing-blazor-webassembly-apps/)
    - `ClientAuth0`と`WebApiAuth0`で実装している
    - クライアント側は`Auth0`で認証できている
    - `TODO`: `Blazor`から`WebApi`にアクセスするときに正しく認証が通らない
    - `TODO`: [公式ページ](https://auth0.com/blog/how-to-validate-jwt-dotnet/)によると`.NET7`以降だともっとシンプルに書ける模様

## データベース初期化

- `WebApi`プロジェクト配下でコマンドを実行し、同プロジェクト配下に`app.db`が生成される

```shell
dotnet dotnet-ef migrations add Initialize
dotnet dotnet-ef database update
```

```shell
efm add Initialize
efd update
```

## Blazor

```shell
dotnet new blazorwasm -o BlazorJwtAuth.Client -f net6.0 --pwa
```

### bunit

```shell
dotnet new --install bunit.template
dotnet new bunit --framework xunit -o <NAME OF TEST PROJECT>
```
