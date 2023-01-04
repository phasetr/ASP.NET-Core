using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Models;

public class Movie
{
    public int Id { get; set; }
    public string? Title { get; set; }

    [DataType(DataType.Date)] public DateOnly ReleaseDate { get; set; }

    public string? Genre { get; set; }
    [Column(TypeName = "decimal(18,4)")] public decimal Price { get; set; }
}