using System.ComponentModel.DataAnnotations;

namespace Common.Models;

public class PersonModel : IModel
{
    public DateTime BirthDay { get; set; }
    public string BirthPlace { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;
    public int[] MoviesIds { get; set; } = Array.Empty<int>();
    public int Id { get; set; }

    [Required] public string Name { get; set; } = string.Empty;
}
