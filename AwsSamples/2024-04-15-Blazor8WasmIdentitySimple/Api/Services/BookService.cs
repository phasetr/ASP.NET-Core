using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Common;
using Common.Dtos;
using Common.Entities;
using Microsoft.Extensions.Options;

namespace Api.Services;

public class BookService : IBookService
{
    private readonly Table _table;

    public BookService(IAmazonDynamoDB dynamoDbClient, IOptions<MyDynamoDbSettings> dynamoDbSettings)
    {
        var tableName = dynamoDbSettings.Value.TableName;
        _table = Table.LoadTable(dynamoDbClient, tableName);
    }

    public async Task<Book?> GetItemAsync(string partitionKey)
    {
        var book = await _table.GetItemAsync(partitionKey, partitionKey);
        if (book == null) return null;
        return new Book
        {
            PartitionKey = book["PartitionKey"],
            Title = book["Title"],
            Isbn = book["Isbn"],
            Authors = book["Authors"].AsListOfString(),
            CoverPage = book["CoverPage"]
        };
    }

    public async Task<string> SaveItemAsync(BookDto dto)
    {
        var bookId = $"BOOK#{Guid.NewGuid().ToString()}";
        var document = new Document
        {
            ["PartitionKey"] = bookId,
            ["SortKey"] = bookId,
            ["Title"] = dto.Title,
            ["Isbn"] = dto.Isbn,
            ["Authors"] = dto.Authors,
            ["CoverPage"] = dto.CoverPage
        };
        await _table.PutItemAsync(document);
        return document["PartitionKey"];
    }
}
