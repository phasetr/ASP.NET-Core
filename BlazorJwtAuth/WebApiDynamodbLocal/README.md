# README

## `DynamoDBContext`

- [`DynamoDBContext`クラス](https://docs.aws.amazon.com/ja_jp/amazondynamodb/latest/developerguide/DotNetDynamoDBContext.html)
- [`dynamodb-admin](https://laid-back-scientist.com/dynamodb-local#toc4)
  - <http://localhost:8001>にアクセスすると`docker-admin`にアクセスできる
  - 必要に応じてアクセスしてテーブルを作る

## `DynamoDB`

- [ASP.NET Core Web API + DynamoDB Locally](https://www.codeproject.com/Articles/5273030/ASP-NET-Core-Web-API-plus-DynamoDB-Locally)
- テーブル作成(未検証)

```shell
aws dynamodb create-table --table-name ECommerce \
  --attribute-definitions AttributeName=PK,AttributeType=S \
  --key-schema AttributeName=PK,KeyType=HASH \
  --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5 \
  --endpoint-url=http://localhost:8000
```

- `GSI`を追加

```shell
aws dynamodb update-table \
  --table-name ECommerce \
  --attribute-definitions AttributeName=GSI1PK,AttributeType=S AttributeName=GSI1SK,AttributeType=S \
  --global-secondary-index-updates \
    '[{"Create":{"IndexName": "GSI1","KeySchema":[{"AttributeName":"GSI1PK","KeyType":"HASH"},{"AttributeName":"GSI1SK","KeyType":"RANGE"}], "ProvisionedThroughput": {"ReadCapacityUnits": 5, "WriteCapacityUnits": 5 }, "Projection":{"ProjectionType":"ALL"}}}]' \
  --endpoint-url=http://localhost:8000
```

- テーブルの確認

```shell
aws dynamodb list-tables --endpoint-url http://localhost:8000
 ```

- 投入データの確認

```shell
aws dynamodb get-item \
  --table-name ECommerce \
  --key '{ "PK": { "S": "CUSTOMER#user1" }, "SK": { "S": "CUSTOMER#user1" } }' \
  --endpoint-url http://localhost:8000
```

```shell
aws dynamodb scan --table-name ECommerce --endpoint-url http://localhost:8000
```
