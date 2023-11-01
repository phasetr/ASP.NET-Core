using Entities.DynamoDb;

namespace Common.Dto;

public class BookGetResponseDto : ResponseDtoBase
{
    public IEnumerable<Book> Books { get; set; } = new List<Book>();
}
