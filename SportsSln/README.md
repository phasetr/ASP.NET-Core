# README

- [Original](https://github.com/Apress/pro-asp.net-core-6/tree/main/11%20-%20SportsStore%20-%205/End%20of%20Chapter/SportsSln)

## TODO
- `dotnet tool install Microsoft.Web.LibraryManager.Cli`の挙動：マニフェストファイルからのインストール

## init

```shell
dotnet libman restore
dotnet restore
# docker起動
dc up -d

dotnet dotnet-ef migrations add InitialCreate -c StoreDbContext
dotnet dotnet-ef migrations add InitialCreate -c AppIdentityDbContext
dotnet dotnet-ef database update -c StoreDbContext
dotnet dotnet-ef database update -c AppIdentityDbContext
```

## Run in development

```shell
# docker起動
dc up -d
dotnet watch run
```