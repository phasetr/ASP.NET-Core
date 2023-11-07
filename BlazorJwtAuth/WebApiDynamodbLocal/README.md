# README

- [参考: DynamoDB Book](https://www.dynamodbbook.com/)

## `DynamoDBContext`

- [`DynamoDBContext`クラス](https://docs.aws.amazon.com/ja_jp/amazondynamodb/latest/developerguide/DotNetDynamoDBContext.html)
- [`dynamodb-admin](https://laid-back-scientist.com/dynamodb-local#toc4)
  - <http://localhost:8001>にアクセスすると`docker-admin`にアクセスできる
  - 必要に応じてアクセスしてテーブルを作る

## `DynamoDB`

- [ASP.NET Core Web API + DynamoDB Locally](https://www.codeproject.com/Articles/5273030/ASP-NET-Core-Web-API-plus-DynamoDB-Locally)

### テスト用の`DynamoDB`のテーブル削除

- 今あるテーブルを取得するコマンドは下記の通り

```shell
aws dynamodb list-tables --endpoint-url http://localhost:8000 --query 'TableNames'
```

- 今あるテーブルのうち、`Z-*`で始まるテーブルを削除するコマンドは下記の通り：`Z-`で始まるテーブルはテスト用のテーブルとしている

```shell
aws dynamodb list-tables --endpoint-url http://localhost:8000 --query 'TableNames' \
  | grep "Z-*" \
  | sed "s/,//g" \
  | xargs -I {} aws dynamodb delete-table --endpoint-url http://localhost:8000 --table-name {} > /dev/null
```

### `SessionStore`

- テーブル作成

```shell
aws dynamodb create-table \
    --table-name SessionStore \
    --endpoint-url=http://localhost:8000 \
    --attribute-definitions \
      AttributeName=PK,AttributeType=S \
      AttributeName=SK,AttributeType=S \
      AttributeName=GSI1PK,AttributeType=S \
      AttributeName=GSI1SK,AttributeType=S \
    --key-schema \
        AttributeName=PK,KeyType=HASH \
        AttributeName=SK,KeyType=RANGE \
    --provisioned-throughput \
        ReadCapacityUnits=5,WriteCapacityUnits=5 \
    --global-secondary-indexes \
        '[
            {
                "IndexName": "GSI1",
                "KeySchema": [
                    { "AttributeName": "GSI1PK", "KeyType": "HASH" },
                    { "AttributeName": "GSI1SK", "KeyType": "RANGE" }
                ],
                "Projection": {
                    "ProjectionType": "ALL"
                },
                "ProvisionedThroughput": {
                    "ReadCapacityUnits": 5,
                    "WriteCapacityUnits": 5
                }
            }
        ]'
```

### `ECommerce`

#### テーブル構造

- **Primary key:** `PK` & `SK`
- **GSI1 secondary index:** `GSI1PK` & `GSI1SK`

| **Entity**    | **PK**                          | **SK**                          | **GSI1PK**        | **GSI1SK**        |
|---------------|---------------------------------|---------------------------------|-------------------|-------------------|
| Customer      | `CUSTOMER#<Username>`           | `CUSTOMER#<Username>`           |                   |                   |
| CustomerEmail | `CUSTOMEREMAIL#<Email>`         | `CUSTOMEREMAIL#<Email>`         |                   |                   |
| Order         | `CUSTOMER#<Username>`           | `#ORDER#<OrderId>`              | `ORDER#<OrderId>` | `ORDER#<OrderId>` |
| OrderItem     | `ORDER#<OrderId>#ITEM#<ItemId>` | `ORDER#<OrderId>#ITEM#<ItemId>` | `ORDER#<OrderId>` | `Item#<ItemId>`   |

#### テーブル作成コマンド

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
  --attribute-definitions \
    AttributeName=GSI1PK,AttributeType=S \
    AttributeName=GSI1SK,AttributeType=S \
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
