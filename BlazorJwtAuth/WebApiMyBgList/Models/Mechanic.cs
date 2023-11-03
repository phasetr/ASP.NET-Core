using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiMyBgList.Models;

[Table("Mechanics")]
public class Mechanic
{
    [Key] [Required] public int Id { get; set; }
    [Required] [MaxLength(200)] public string Name { get; set; } = null!;
    [Required] public DateTime CreatedDate { get; set; }
    [Required] public DateTime LastModifiedDate { get; set; }
    public ICollection<BoardGamesMechanics>? BoardGamesMechanics { get; set; }
}
