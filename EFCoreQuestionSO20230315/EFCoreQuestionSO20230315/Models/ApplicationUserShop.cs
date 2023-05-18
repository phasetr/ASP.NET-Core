namespace EFCoreQuestionSO20230315.Models;

public class ApplicationUserShop
{
    public string ApplicationUserId { get; set; } = default!;
    public ApplicationUser ApplicationUser { get; set; } = default!;
    public int ShopId { get; set; }
    public Shop Shop { get; set; } = default!;
}
