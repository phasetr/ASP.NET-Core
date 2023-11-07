namespace WebApiDynamodbLocal.Models.BigTimeDeals;

public class BrandModel
{
    public string Name { get; set; } = default!;
    public string LogoUrl { get; set; } = default!;
    public int LikeCount { get; set; } = default!;
    public int WatchCount { get; set; } = default!;
}
