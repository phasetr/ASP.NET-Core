using IdentityByController.Models;
using IdentityByController.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace IdentityByController.Controllers;

public class HomeController : Controller
{
    private readonly IStoreRepository _repository;
    public int PageSize = 4;

    public HomeController(IStoreRepository repo)
    {
        _repository = repo;
    }

    public ViewResult Index(string? category, int productPage = 1)
    {
        return View(new ProductsListViewModel
        {
            Products = _repository.Products
                .Where(p => category == null || p.Category == category)
                .OrderBy(p => p.ProductId)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize),
            PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = category == null
                    ? _repository.Products.Count()
                    : _repository.Products.Count(e => e.Category == category)
            },
            CurrentCategory = category
        });
    }
}