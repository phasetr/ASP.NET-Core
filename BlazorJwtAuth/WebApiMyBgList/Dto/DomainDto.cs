using System.ComponentModel.DataAnnotations;

namespace WebApiMyBgList.Dto;

public class DomainDto
{
    [Required] public int Id { get; set; }

    public string? Name { get; set; }
}
