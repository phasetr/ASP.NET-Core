using Common.Dtos;
using Common.Entities;

namespace Api.Services;

public interface IBookService
{
    Task<BookResponseDto?> GetItemAsync(string partitionKey);
    Task<List<BookResponseDto>> GetListAsync();
    Task<string> SaveItemAsync(BookDto dto);
}
