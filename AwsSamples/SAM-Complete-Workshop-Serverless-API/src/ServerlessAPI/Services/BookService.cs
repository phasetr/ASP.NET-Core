using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using ServerlessAPI.Entities;

namespace ServerlessAPI.Services;

public class BookService : IBookService
{
    private readonly IDynamoDBContext _context;
    private readonly ILogger<BookService> _logger;

    public BookService(IDynamoDBContext context, ILogger<BookService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> CreateAsync(Book book)
    {
        try
        {
            book.Id = Guid.NewGuid();
            await _context.SaveAsync(book);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to persist to DynamoDb Table");
            return false;
        }

        return true;
    }

    public async Task<bool> DeleteAsync(Book book)
    {
        bool result;
        try
        {
            // Delete the book.
            await _context.DeleteAsync<Book>(book.Id);
            // Try to retrieve deleted book. It should return null.
            var deletedBook = await _context.LoadAsync<Book>(book.Id, new DynamoDBContextConfig
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

        if (result) _logger.LogInformation("Book {@Id} is deleted", book);

        return result;
    }

    public async Task<bool> UpdateAsync(Book? book)
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

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _context.LoadAsync<Book>(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to update book from DynamoDb Table");
            return null;
        }
    }

    public async Task<IList<Book>> GetBooksAsync(int limit = 10)
    {
        var result = new List<Book>();

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
            var queryResult = _context.FromScanAsync<Book>(scanConfig);

            do
            {
                result.AddRange(await queryResult.GetNextSetAsync());
            } while (!queryResult.IsDone && result.Count < limit);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to list books from DynamoDb Table");
            return new List<Book>();
        }

        return result;
    }
}
