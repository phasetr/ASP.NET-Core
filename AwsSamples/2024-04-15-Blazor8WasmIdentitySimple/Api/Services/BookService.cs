using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Common;
using Common.Dtos;
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

    public async Task<BookResponseDto?> GetItemAsync(string partitionKey)
    {
        var book = await _table.GetItemAsync(partitionKey, partitionKey);
        if (book == null) return null;
        return new BookResponseDto
        {
            BookId = book["PartitionKey"],
            Title = book["Title"],
            Isbn = book["Isbn"],
            Authors = book["Authors"].AsListOfString(),
            CoverPage = book["CoverPage"]
        };
    }

    public async Task<List<BookResponseDto>> GetListAsync()
    {
        // TODO 適当な形でクエリフィルターにしたい
        var scanFilter = new ScanFilter();
        scanFilter.AddCondition(
            MyConstants.DynamoDbPartitionKey,
            ScanOperator.Contains,
            "BOOK#");
        var search = _table.Scan(scanFilter);
        var bookResponseDtos = new List<BookResponseDto>();
        do
        {
            var documentSet = await search.GetNextSetAsync();
            bookResponseDtos.AddRange(documentSet.Select(document => new BookResponseDto
            {
                BookId = document["PartitionKey"],
                Title = document["Title"],
                Isbn = document["Isbn"],
                Authors = document["Authors"].AsListOfString(),
                CoverPage = document["CoverPage"]
            }));
        } while (!search.IsDone);

        return bookResponseDtos;
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
