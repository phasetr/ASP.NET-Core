# README

- Azure Portal: <https://portal.azure.com/>
- [awesome-azd](https://azure.github.io/awesome-azd/)
  - `.NET/C#`で`C#`系の[`Azure Developer CLI`](https://learn.microsoft.com/ja-jp/azure/developer/azure-developer-cli/)の様々なサンプルがある
  - 以下のサンプルが特に参考になる
    - `.NET eShop on Web App with Redis Cache`：`azd init -t cawams/eshop`
    - `Blazor Web App with C# and SQL Database on Azure`：`azd init -t jasontaylordev/azd-blazor`
      - `Blazor Server(App Service)+SQL Server(Azure SQL)`
    - `React Web App with C# API and MongoDB`：`azd init -t todo-csharp-cosmos-sql`
    - `React Web App with C# API and SQL Database`：`azd init -t todo-csharp-sql`
    - `Static React Web App + Functions with C# API and SQL Database`：`azd init -t todo-csharp-sql-swa-func`
- [Bicepのドキュメント](https://learn.microsoft.com/ja-jp/azure/azure-resource-manager/bicep/)
- [Bicep関数](https://learn.microsoft.com/ja-jp/azure/azure-resource-manager/bicep/bicep-functions)
- [Bicepの基礎](https://learn.microsoft.com/ja-jp/training/paths/fundamentals-bicep/)
- 公式：[Bicep CLIコマンド](https://learn.microsoft.com/ja-jp/azure/azure-resource-manager/bicep/bicep-cli)
- [Azureクイックスタートテンプレート](Azure クイックスタート テンプレート )
- Book: [Infrastructure as Code with Azure Bicep](https://www.packtpub.com/product/infrastructure-as-code-with-azure-bicep/9781801813747)
  - GitHub: [PacktPublishing / Infrastructure-as-Code-with-Azure-Bicep](https://github.com/PacktPublishing/Infrastructure-as-Code-with-Azure-Bicep)
- 公式：[Azure Functions](https://learn.microsoft.com/ja-jp/azure/azure-functions/)
- サンプル：[serverless-web-application](https://github.com/Azure-Samples/serverless-web-application/tree/main)

## Bicepインストール

```shell
az bicep install && az bicep upgrade
```

## リソースへの命名規則

- [名前付け規則を定義する](https://learn.microsoft.com/ja-jp/azure/cloud-adoption-framework/ready/azure-best-practices/resource-naming)
- [弊社で使っているAzureリソースのスルメ系命名規則を紹介します](https://zenn.dev/aeonpeople/articles/0b4a4be83d0dfd)

## Microsoft Azure の基礎

- [パート 1: クラウドの概念について説明する](https://learn.microsoft.com/ja-jp/training/paths/microsoft-azure-fundamentals-describe-cloud-concepts/)
- [パート 2: Azure のアーキテクチャとサービスについて説明する](https://learn.microsoft.com/ja-jp/training/paths/azure-fundamentals-describe-azure-architecture-services/)
- [パート 3: Azure の管理とガバナンスについて説明する](https://learn.microsoft.com/ja-jp/training/paths/describe-azure-management-governance/)

## Azure Functions

- Microsoft Build：[対話型チュートリアル一覧](https://learn.microsoft.com/ja-jp/training/browse/?expanded=azure&products=azure-functions)

## TODO

- azure-quickstart-templates, [/cosmosdb-webapp](https://github.com/Azure/azure-quickstart-templates/tree/master/quickstarts/microsoft.documentdb/cosmosdb-webapp)
- [ガイド付きプロジェクト - Azure Cosmos DB for NoSQL を使用して .NET アプリを構築する](https://learn.microsoft.com/ja-jp/training/modules/build-dotnet-app-azure-cosmos-db-nosql/)
- ラーニングパス：[Azure Static Web Apps](https://learn.microsoft.com/ja-jp/training/paths/azure-static-web-apps/)
- ラーニングパス：[Bicep と Azure Pipelines を使用して Azure リソースをデプロイ](https://learn.microsoft.com/ja-jp/training/paths/bicep-azure-pipelines/)
- ラーニングパス：[Bicep アクションと GitHub アクションを使用して Azure リソースをデプロイする](https://learn.microsoft.com/ja-jp/training/paths/bicep-github-actions/)
- [GitHub Actions を使って CI/CD を実装し、Bicep を使って IaC を実装する](https://learn.microsoft.com/ja-jp/training/modules/deliver-with-devops/6-implement-ci-cd-with-github-actions-and-infrastructure-as-code-with-bicep)
- [AZ-400: Azure Pipelines と GitHub Actions での CI の実装](https://learn.microsoft.com/ja-jp/training/paths/az-400-implement-ci-azure-pipelines-github-actions/)
- [開発プロセスを合理化する目的で Azure DevOps と GitHub をいろいろ試す](https://learn.microsoft.com/ja-jp/training/paths/explore-azure-devops-with-github/)
