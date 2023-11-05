using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using KsuidDotNet;

namespace Test.WebApiDynamodbLocal.Unit.FakesDynamoDb;

public class AmazonDynamoDbClientFakeBuilder : IDisposable
{
    private readonly AmazonDynamoDBClient _client;
    private readonly string _tableName;

    public AmazonDynamoDbClientFakeBuilder()
    {
        var clientConfig = new AmazonDynamoDBConfig {ServiceURL = "http://localhost:8000"};
        _client = new AmazonDynamoDBClient(clientConfig);
        var dateTime = DateTime.UtcNow;
        var ksuId = Ksuid.NewKsuid(dateTime);
        _tableName = $"Z-{ksuId}";

        var request = new CreateTableRequest
        {
            TableName = _tableName,
            AttributeDefinitions = new List<AttributeDefinition>
            {
                new() {AttributeName = "PK", AttributeType = ScalarAttributeType.S},
                new() {AttributeName = "SK", AttributeType = ScalarAttributeType.S},
                new() {AttributeName = "GSI1PK", AttributeType = ScalarAttributeType.S},
                new() {AttributeName = "GSI1SK", AttributeType = ScalarAttributeType.S}
            },
            KeySchema = new List<KeySchemaElement>
            {
                new() {AttributeName = "PK", KeyType = KeyType.HASH},
                new() {AttributeName = "SK", KeyType = KeyType.RANGE}
            },
            ProvisionedThroughput = new ProvisionedThroughput
            {
                ReadCapacityUnits = 5,
                WriteCapacityUnits = 5
            },
            GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
            {
                new()
                {
                    IndexName = "GSI1",
                    KeySchema = new List<KeySchemaElement>
                    {
                        new() {AttributeName = "GSI1PK", KeyType = KeyType.HASH},
                        new() {AttributeName = "GSI1SK", KeyType = KeyType.RANGE}
                    },
                    Projection = new Projection {ProjectionType = ProjectionType.ALL},
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5
                    }
                }
            }
        };
        _client.CreateTableAsync(request).Wait();
    }

    public void Dispose()
    {
        // テスト用に一時的に作ったテーブルは`README.md`に記載のコマンドで削除する
        // var request = new DeleteTableRequest {TableName = _tableName};
        // _client.DeleteTableAsync(request).Wait();
        _client.Dispose();
    }

    public ReturnValue Build()
    {
        return new ReturnValue
        {
            Client = _client,
            TableName = _tableName
        };
    }

    public class ReturnValue
    {
        public AmazonDynamoDBClient Client { get; set; } = default!;
        public string TableName { get; set; } = default!;
    }
}
