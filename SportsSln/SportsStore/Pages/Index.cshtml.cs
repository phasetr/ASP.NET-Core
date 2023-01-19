using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Pages;

public class Index : PageModel
{
    private readonly IStoreRepository _repository;
    private readonly ILogger<Index> _logger;
    public int PageSize = 4;

    public Index(IStoreRepository repo, ILogger<Index> logger)
    {
        _repository = repo;
        _logger = logger;
    }

    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
    public PagingInfo PagingInfo { get; set; } = new();
    public string? CurrentCategory { get; set; }

    public void OnGet(string? category, int productPage = 1)
    {
        Products = _repository.Products
            .Where(p => category == null || p.Category == category)
            .OrderBy(p => p.ProductID)
            .Skip((productPage - 1) * PageSize)
            .Take(PageSize);
        PagingInfo = new PagingInfo
        {
            CurrentPage = productPage,
            ItemsPerPage = PageSize,
            TotalItems = category == null
                ? _repository.Products.Count()
                : _repository.Products.Count(e => e.Category == category)
        };
        CurrentCategory = category;
        _logger.LogInformation("LOG TEST");
        _logger.LogInformation($"{PagingInfo.CurrentPage},{PagingInfo.ItemsPerPage},{PagingInfo.TotalItems}");
        _logger.LogInformation("{CurrentCategory}", CurrentCategory);
    }
}