using System.ComponentModel.DataAnnotations;

namespace WebApiMyBgList.Models;

public class BoardGamesDomains
{
    [Key] [Required] public int BoardGameId { get; set; }
    [Key] [Required] public int DomainId { get; set; }
    [Required] public DateTime CreatedDate { get; set; }
    public BoardGame? BoardGame { get; set; }
    public Domain? Domain { get; set; }
}
