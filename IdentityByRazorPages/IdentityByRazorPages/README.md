# README

## ID/PASS

- Admin/Secret123$

## APIアクセス

- <https://localhost:5500/webclient.html>
- 同じブラウザでクッキーログインしていると`JWT`認証なしでもAPIリクエストが成功する点に注意する

## restore

```shell
dotnet tool restore
dotnet libman restore
dotnet restore
```

## データベース初期化

```shell
# dotnet dotnet-ef migrations add InitialCreate -c MyDbContext && dotnet dotnet-ef migrations add InitialCreate -c IdContext
dotnet dotnet-ef database update -c MyDbContext && dotnet dotnet-ef database update -c IdContext
```
