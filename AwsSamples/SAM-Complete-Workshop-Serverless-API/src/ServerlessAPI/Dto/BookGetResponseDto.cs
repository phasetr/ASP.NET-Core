using ServerlessAPI.Entities;

namespace ServerlessAPI.Dto;

public class BookGetResponseDto : ResponseDtoBase
{
    public IEnumerable<Book> Books { get; set; } = new List<Book>();
}
