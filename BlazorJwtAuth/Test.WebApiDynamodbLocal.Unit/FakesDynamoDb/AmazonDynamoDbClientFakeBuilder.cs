using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace Test.WebApiDynamodbLocal.Unit.FakesDynamoDb;

public class AmazonDynamoDbClientFakeBuilder : IDisposable
{
    private readonly AmazonDynamoDBClient _client;
    private readonly string _tableName;

    public AmazonDynamoDbClientFakeBuilder()
    {
        var clientConfig = new AmazonDynamoDBConfig {ServiceURL = "http://localhost:8000"};
        _client = new AmazonDynamoDBClient(clientConfig);
        _tableName = Guid.NewGuid().ToString();

        var request = new CreateTableRequest
        {
            TableName = _tableName,
            AttributeDefinitions = new List<AttributeDefinition>
            {
                new() {AttributeName = "PK", AttributeType = ScalarAttributeType.S},
                new() {AttributeName = "SK", AttributeType = ScalarAttributeType.S}
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
            }
        };
        _client.CreateTableAsync(request).Wait();
    }

    public void Dispose()
    {
        // TODO: 一時的に作ったテーブルが消えるようにする
        var request = new DeleteTableRequest {TableName = _tableName};
        _client.DeleteTableAsync(request).Wait();
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
