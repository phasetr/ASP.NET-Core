namespace Common.Dtos;

public class BookResponseDto
{
    public string BookId { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string? Isbn { get; init; }
    public List<string>? Authors { get; init; }
    public string? CoverPage { get; init; }
}
