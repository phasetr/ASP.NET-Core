using System.ComponentModel.DataAnnotations;

namespace MediaLibrary.Common.Entities;

public class BaseEntity
{
    [Key] public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
