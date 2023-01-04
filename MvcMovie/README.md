# README

- [公式](https://learn.microsoft.com/ja-jp/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-6.0&tabs=visual-studio)
- [ASP.NET Core 6 MVCのチュートリアルを優しく解説](https://masa7blog.com/asp-net-core-6-mvc-tutorial/)

## EF Core

### インストール

```shell
dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef --version 6.0.12
```

### マイグレーション

```shell
dotnet ef migrations add InitialCreate
```

### アップデート

```shell
dotnet ef database update
```