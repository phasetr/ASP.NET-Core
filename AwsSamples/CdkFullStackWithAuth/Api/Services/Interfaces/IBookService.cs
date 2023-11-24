using Common.Entities;

namespace Api.Services.Interfaces;

/// <summary>
///     Sample DynamoDB Table book CRUD
/// </summary>
public interface IBookService
{
    /// <summary>
    ///     Include new book to the DynamoDB Table
    /// </summary>
    /// <param name="bookEntity">book to include</param>
    /// <returns>success/failure</returns>
    Task<bool> CreateAsync(BookEntity bookEntity);

    /// <summary>
    ///     Remove existing book from DynamoDB Table
    /// </summary>
    /// <param name="bookEntity">book to remove</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(BookEntity bookEntity);

    /// <summary>
    ///     List book from DynamoDb Table with items limit (default=10)
    /// </summary>
    /// <param name="limit">limit (default=10)</param>
    /// <returns>Collection of books</returns>
    Task<IList<BookEntity>> GetBooksAsync(int limit = 10);

    /// <summary>
    ///     Get book by PK
    /// </summary>
    /// <param name="id">book`s PK</param>
    /// <returns>Book object</returns>
    Task<BookEntity?> GetByIdAsync(Guid id);

    /// <summary>
    ///     Update book content
    /// </summary>
    /// <param name="bookEntity">book to be updated</param>
    /// <returns></returns>
    Task<bool> UpdateAsync(BookEntity bookEntity);
}
