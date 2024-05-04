# README

- `F#`と`EF Core`の食い合わせが悪いため、`DataContext`プロジェクトを`C#`で作る必要がある

## メモ

- テストは[Giraffeのサンプル](https://github.com/giraffe-fsharp/samples/tree/master/demo-apps/SampleApp/SampleApp.Tests)が参考になりそう

## `EF Core`

### マイグレーション作成

- `DataContext`配下に`Migrations`ができる

```shell
dotnet ef migrations add <SomeName> --project DataContext --startup-project Api
```

### マイグレーションの適用

```shell
dotnet ef database update --project DataContext --startup-project Api
```
