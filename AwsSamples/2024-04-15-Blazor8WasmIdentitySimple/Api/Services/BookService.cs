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
            Isbn = book.TryGetValue("Isbn", out var isbn) ? isbn : string.Empty,
            Authors = book["Authors"].AsListOfString(),
            CoverPage = book.TryGetValue("CoverPage", out var coverPage) ? coverPage : string.Empty
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
                Isbn = document.TryGetValue("Isbn", out var isbn) ? isbn : string.Empty,
                Authors = document["Authors"].AsListOfString(),
                CoverPage = document.TryGetValue("CoverPage", out var coverPage) ? coverPage : string.Empty
            }));
        } while (!search.IsDone);

        return bookResponseDtos;
    }

    public async Task<string> PutItemAsync(BookPutDto putDto)
    {
        var bookId = $"BOOK#{Guid.NewGuid().ToString()}";
        var document = new Document
        {
            ["PartitionKey"] = bookId,
            ["SortKey"] = bookId,
            ["Title"] = putDto.Title,
            ["Isbn"] = putDto.Isbn,
            ["Authors"] = putDto.Authors,
            ["CoverPage"] = putDto.CoverPage
        };
        await _table.PutItemAsync(document);
        return document["PartitionKey"];
    }

    public async Task<bool> UpdateItemAsync(BookUpdateDto putDto)
    {
        try
        {
            var document = new Document
            {
                ["PartitionKey"] = putDto.BookId,
                ["SortKey"] = putDto.BookId,
                ["Title"] = putDto.Title,
                ["Isbn"] = putDto.Isbn,
                ["Authors"] = putDto.Authors,
                ["CoverPage"] = putDto.CoverPage
            };
            await _table.UpdateItemAsync(document);
            return true;
        } catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            return false;
        }
    }
}
