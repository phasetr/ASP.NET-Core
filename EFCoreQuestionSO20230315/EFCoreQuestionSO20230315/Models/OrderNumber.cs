using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreQuestionSO20230315.Models;

public class OrderNumber
{
    [Key] public int ShopId { get; set; }

    [ForeignKey("ShopId")] public Shop Shop { get; set; } = default!;

    public int Number { get; set; }
}
