using System.ComponentModel.DataAnnotations;

namespace EFCoreQuestionSO20230315.Models;

public class PaymentMethod
{
    [Required] public int ShopId { get; set; }
    public Shop Shop { get; set; } = default!;
    [Required] [MaxLength(255)] public string Name { get; set; } = "";
}
