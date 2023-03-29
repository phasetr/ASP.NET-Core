using System.ComponentModel.DataAnnotations;

namespace EFCoreQuestionSO20230315.Models;

public class OrderNumber
{
    [Key] public int ShopId { get; set; }
    public Shop Shop { get; set; } = default!;
    public int Number { get; set; }
}
