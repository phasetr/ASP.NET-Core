namespace Common.Entities;

public class Book
{
    public string PartitionKey { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Isbn { get; set; }
    public List<string>? Authors { get; set; }
    public string? CoverPage { get; set; }
}
