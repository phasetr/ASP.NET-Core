using Common.Entities;

namespace Common.Dto;

public class BookGetDto : ResponseBaseDto
{
    public IEnumerable<BookEntity> Books { get; set; } = new List<BookEntity>();
}
