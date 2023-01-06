# README

- [Original](https://github.com/Apress/pro-asp.net-core-6/tree/main/11%20-%20SportsStore%20-%205/End%20of%20Chapter/SportsSln)
- ストア用と認証用でデータベースが二つある.

## サイト

- 要ログインページ：[管理者ページ](http://localhost:5000/admin)または[管理者情報ページ](http://localhost:5000/admin/identityusers): 
    - `ID/PASS = Admin/Secret123$`
    - `ID/PASS`は`Models/IdentitySeedData.cs`で設定している
    - 直接<http://localhost:5000/Account/Login>に遷移するとエラーになる：`GET`パラメーター`ReturnUrl`が必要

## init
### `restore`
```shell
dotnet libman restore
dotnet restore
```
### docker起動
```shell
docker compose up -d # バックグラウンド起動
```

- ダウンしたいときは`docker compose down`

### データベース初期化
- 標準は`PostgreSQL`でその前提でマイグレーションファイルが生成されている
- 他のデータベースにしたい場合はマイグレーションファイルを削除してからコメントアウトされた`migrations add`の二行を実行すれば良い

```shell
# dotnet dotnet-ef database drop --force --context StoreDbContext
# dotnet dotnet-ef database drop --force --context AppIdentityDbContext
# dotnet dotnet-ef migrations add InitialCreate -c StoreDbContext
# dotnet dotnet-ef migrations add InitialCreate -c AppIdentityDbContext
dotnet dotnet-ef database update -c StoreDbContext
dotnet dotnet-ef database update -c AppIdentityDbContext
```

## Run in development

```shell
# docker起動
dc up -d
dotnet watch run
```
