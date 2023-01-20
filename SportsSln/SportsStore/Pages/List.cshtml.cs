using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using static SportsStore.Models.Constants;

namespace SportsStore.Pages;

public class ListModel : PageModel
{
    private readonly ILogger<ListModel> _logger;
    private readonly IStoreRepository _repository;

    public ListModel(IStoreRepository repo, ILogger<ListModel> logger)
    {
        _repository = repo;
        _logger = logger;
    }

    public List<string> Categories { get; set; } = TemporalCategories;

    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
    public PagingInfo PagingInfo { get; set; } = new();
    public string? CurrentCategory { get; set; }

    public void OnGet(string? category, int productPage = 1)
    {
        _logger.LogInformation("{Category}, {ProductPage}", category, productPage);
        Products = _repository.Products
            .Where(p => category == null || p.Category == category)
            .OrderBy(p => p.ProductID)
            .Skip((productPage - 1) * DefaultPageSize)
            .Take(DefaultPageSize);
        var totalItems = category == null
            ? _repository.Products.Count()
            : _repository.Products.Count(e => e.Category == category);
        PagingInfo = new PagingInfo
        {
            CurrentPage = productPage,
            ItemsPerPage = DefaultPageSize,
            TotalItems = totalItems
        };
        CurrentCategory = category;
    }
}