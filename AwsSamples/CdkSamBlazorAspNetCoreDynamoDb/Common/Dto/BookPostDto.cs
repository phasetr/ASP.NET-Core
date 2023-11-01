using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Common.Dto;

public class BookPostDto
{
    [DefaultValue("Title1")] [Required] public string Title { get; set; } = default!;
    [DefaultValue("ISBN")] public string? Isbn { get; set; }

    [DefaultValue(new[] {"Author1", "Author2"})]
    public List<string>? Authors { get; set; }
}
