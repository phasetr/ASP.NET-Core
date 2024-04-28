namespace Common.Dtos;

public class BookUpdateDto
{
    public string BookId { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Isbn { get; set; }
    public List<string>? Authors { get; set; }
    public string? CoverPage { get; set; }
}
