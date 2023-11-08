namespace WebApiDynamodbLocal.Models.BigTimeDeals;

public class CategoryModel
{
    public string Name { get; set; } = default!;
    public string FeaturedDeals { get; set; } = default!;
    public int LikeCount { get; set; }
    public int WatchCount { get; set; }
}
