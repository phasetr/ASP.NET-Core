# README

## 開発用データベース設定
### PostgreSQL

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "User ID=user;Password=pass;Host=localhost;Port=5432;Database=mydb;"
  }
}
```

## SQLite

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "DataSource=app.db;Cache=Shared"
  }
}
```

## dotnet init

```shell
dotnet new tool-manifest
dotnet tool install dotnet-ef --version 6.0.15
dotnet restore
dotnet tool restore
dotnet dotnet-ef database update
```

## コマンドサンプル

- `Route 53`を含めドメイン関係の設定をしておく.

```
copilot app init aspdotnet --domain math-accessory.com
```

```
copilot env init --name staging \
  --profile default \
  --app aspdotnet \
  --default-config
```

```
copilot svc init --name aspdotnet \
  --svc-type "Load Balanced Web Service" \
  --dockerfile Dockerfile \
  --port 80
```

```
copilot storage init -n aspdotnet-cluster \
  -l workload \
  -t Aurora \
  -w aspdotnet \
  --engine PostgreSQL \
  --initial-db mydb
```

- `git`で差分を見て適切に修正する

```
copilot env deploy
```

```
copilot svc deploy
```

```
copilot svc show
```

```
copilot svc deploy
```

```
copilot job init -n dbmigrate
```

```
copilot job deploy --name dbmigrate --env staging
```

```
copilot job run --name dbmigrate --env staging
```
