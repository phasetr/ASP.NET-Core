using Common.Dtos;
using Common.Entities;

namespace Api.Services;

public interface IBookService
{
    Task<Book?> GetItemAsync(string partitionKey);
    Task<string> SaveItemAsync(BookDto dto);
}
