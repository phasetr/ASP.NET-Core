namespace Common.Dtos;

public class BookPutDto
{
    public string Title { get; set; } = default!;
    public string? Isbn { get; set; }
    public List<string>? Authors { get; set; }
    public string? CoverPage { get; set; }
}
