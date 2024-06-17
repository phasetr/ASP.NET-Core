# README

## 重大な注意

- `Lambda`+(Websocketの)単純な`API Gateway`では動かない模様
- `Websocket`の設定例としてとりあえず置いておく
- `Blazor`自体はローカルで動く

## デプロイ

- `Docker`を立ち上げる

```shell
cdk deploy --profile dev
```

## 初期化

- `Rider`で`Blazor Web App`+`Individual authentication`で設定
- 詳細で次を設定
  - `Interactive render mode = Global`
  - `Interactivity location = Global`
- `SQLite`の`Identity`も設定済み
- `.gitignore`に`app.db*`を追加
- `appsettings.json`を次のように修正

```
{
  "ConnectionStrings": {
    "DefaultConnection": "DataSource=app.db;Cache=Shared"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

- 必要に応じてマイグレーションする

```shell
dotnet new tool-manifest
dotnet tool install dotnet-ef
dotnet dotnet-ef database update --project 2024-06-17-BlazorServerAuth
```

- `Program.cs`で次の変更を入れる

```csharp
builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        // パスワードポリシーの設定
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();
```

- 起動する
- 正しく動くのを確認してからコードをコミット
- コードを整形して変更点を確認してからもう一度動かし、エラーが起きていないか確認する
  - 特に認証周りで注意 
