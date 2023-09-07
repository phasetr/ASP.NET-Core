# README

- [ORIGINAL](https://github.com/iammukeshm/JWTAuthentication.WebApi)
- [Build Secure ASP.NET Core API with JWT Authentication – Detailed Guide](https://codewithmukesh.com/blog/aspnet-core-api-with-jwt-authentication/)
- [How to Use Refresh Tokens in ASP.NET Core APIs – JWT Authentication](https://codewithmukesh.com/blog/refresh-tokens-in-aspnet-core/)
- 認証
  - [.NET 6.0 JWT Token Authentication C# API Tutorial](https://trystanwilcock.com/2022/08/13/net-6-0-jwt-token-authentication-c-sharp-api-tutorial/) 
  - [.NET 6.0 Blazor WebAssembly JWT Token Authentication From Scratch C# Tutorial](https://trystanwilcock.com/2022/09/28/net-6-0-blazor-webassembly-jwt-token-authentication-from-scratch-c-sharp-wasm-tutorial/)
- 結合テストの参考ページ: [ASP.NET Core で Web API の結合テストをしよう](https://qiita.com/okazuki/items/cbda6c456dcba8fee503) 

## Blazor

```shell
dotnet new blazorwasm -o BlazorJwtAuth.Client -f net6.0 --pwa
```

### bunit

```shell
dotnet new --install bunit.template
dotnet new bunit --framework xunit -o <NAME OF TEST PROJECT>
```
