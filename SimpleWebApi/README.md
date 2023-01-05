# README

- [チュートリアル: ASP.NET Core で Web API を作成する](https://learn.microsoft.com/ja-jp/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio)
- データベースはインメモリで単純化
- `Scaffold`のために（余計な）パッケージ・ツールを導入している

## memo

```shell
dotnet add package Microsoft.EntityFrameworkCore --version 6.0.12
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design -v 6.0.12
dotnet add package Microsoft.EntityFrameworkCore.Design -v 6.0.12
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design -v 6.0.11
dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 6.0.12 # scaffoldで必要
```

```shell
dotnet new tool-manifest
dotnet tool install dotnet-aspnet-codegenerator --version 6.0.11
dotnet dotnet-aspnet-codegenerator controller -name TodoItemsController -async -api -m TodoItem -dc TodoContext -outDir Controllers
```