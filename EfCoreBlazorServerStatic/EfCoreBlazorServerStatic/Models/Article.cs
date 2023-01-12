namespace EfCoreBlazorServerStatic.Models;

public class Article
{
    public int ArticleId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public virtual ApplicationUser? User { get; set; }
}