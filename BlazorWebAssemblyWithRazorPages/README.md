# README

認証つきの`Blazor WebAssembly`+`Razor Pages`サンプル.

```shell
dotnet new blazorwasm -ho --auth Individual --pwa -o BlazorWebAssemblyWithRazorPages
```

## JWT

- [.NET 6.0 - JWT Authentication with Refresh Tokens Tutorial with Example API](https://jasonwatmore.com/post/2022/01/24/net-6-jwt-authentication-with-refresh-tokens-tutorial-with-example-api)

## ルート

- /users/authenticate- 本文にユーザー名とパスワードを含む POST リクエストを受け入れるパブリック ルート。 成功すると、基本的なユーザーの詳細とともに JWT アクセス トークンが返され、更新トークンを含む HTTP のみの Cookie が返されます。
- /users/refresh-token- リフレッシュ トークン付きの Cookie を含む POST 要求を受け入れるパブリック ルート。 成功すると、新しい JWT アクセス トークンが基本的なユーザーの詳細と共に返され、新しい更新トークンを含む HTTP のみの Cookie が返されます ( 更新トークンのローテーションを参照してください)。 説明については、すぐ下の
- /users/revoke-token- 要求本文または Cookie のいずれかに更新トークンを含む POST 要求を受け入れる安全なルート (両方が存在する場合) は、要求本文が優先されます。 成功すると、トークンは取り消され、新しい JWT アクセス トークンの生成に使用できなくなります。
- /users- GET 要求を受け入れ、アプリケーション内のすべてのユーザーのリストを返す安全なルート。
- /users/{id}- GET リクエストを受け入れ、指定された ID を持つユーザーの詳細を返す安全なルート。
- /users/{id}/refresh-tokens- GET リクエストを受け入れ、指定された ID を持つユーザーのすべてのリフレッシュ トークン (アクティブおよび取り消された) のリストを返す安全なルート。
