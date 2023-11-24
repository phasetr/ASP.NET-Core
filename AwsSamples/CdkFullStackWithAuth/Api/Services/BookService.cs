using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Api.Services.Interfaces;
using Common.Entities;

namespace Api.Services;

public class BookService : IBookService
{
    private readonly IDynamoDBContext _context;
    private readonly ILogger<BookService> _logger;

    public BookService(IDynamoDBContext context, ILogger<BookService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> CreateAsync(BookEntity bookEntity)
    {
        try
        {
            bookEntity.Id = Guid.NewGuid();
            await _context.SaveAsync(bookEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to persist to DynamoDb Table");
            return false;
        }

        return true;
    }

    public async Task<bool> DeleteAsync(BookEntity bookEntity)
    {
        bool result;
        try
        {
            // Delete the book.
            await _context.DeleteAsync<BookEntity>(bookEntity.Id);
            // Try to retrieve deleted book. It should return null.
            var deletedBook = await _context.LoadAsync<BookEntity>(bookEntity.Id, new DynamoDBContextConfig
            {
                ConsistentRead = true
            });

            result = deletedBook == null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to delete book from DynamoDb Table");
            result = false;
        }

        if (result) _logger.LogInformation("Book {@Id} is deleted", bookEntity);
        return result;
    }

    public async Task<bool> UpdateAsync(BookEntity? book)
    {
        if (book == null) return false;

        try
        {
            await _context.SaveAsync(book);
            _logger.LogInformation("Book {@Id} is updated", book);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to update book from DynamoDb Table");
            return false;
        }

        return true;
    }

    public async Task<BookEntity?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _context.LoadAsync<BookEntity>(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to update book from DynamoDb Table");
            return null;
        }
    }

    public async Task<IList<BookEntity>> GetBooksAsync(int limit = 10)
    {
        var result = new List<BookEntity>();

        try
        {
            if (limit <= 0) return result;

            var filter = new ScanFilter();
            filter.AddCondition("Id", ScanOperator.IsNotNull);
            var scanConfig = new ScanOperationConfig
            {
                Limit = limit,
                Filter = filter
            };
            var queryResult = _context.FromScanAsync<BookEntity>(scanConfig);

            do
            {
                result.AddRange(await queryResult.GetNextSetAsync());
            } while (!queryResult.IsDone && result.Count < limit);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to list books from DynamoDb Table");
            return new List<BookEntity>();
        }

        return result;
    }
}
