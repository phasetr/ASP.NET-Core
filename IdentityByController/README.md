# README

- [Original](https://github.com/Apress/pro-asp.net-core-6/tree/main/11%20-%20SportsStore%20-%205/End%20of%20Chapter/SportsSln)

## サイト

- 要ログインページ：[管理者ページ](http://localhost:5000/admin)または[管理者情報ページ](http://localhost:5000/admin/identityusers):
  - `ID/PASS = Admin/Secret123$`
  - `ID/PASS`は`Models/Seeds/IdentitySeedData.cs`で設定している
  - 直接<http://localhost:5000/Account/Login>に遷移するとエラーになる：`GET`パラメーター`ReturnUrl`が必要

## init

### `restore`

```shell
dotnet tool restore
dotnet libman restore
dotnet restore
```

### データベース初期化

```shell
# dotnet dotnet-ef database drop --force --context StoreDbContext && dotnet dotnet-ef database drop --force --context AppIdentityDbContext
# dotnet dotnet-ef migrations add InitialCreate -c StoreDbContext && dotnet dotnet-ef migrations add InitialCreate -c AppIdentityDbContext
dotnet dotnet-ef database update -c StoreDbContext && dotnet dotnet-ef database update -c AppIdentityDbContext
```

## Run in development

```shell
dotnet watch run
```
