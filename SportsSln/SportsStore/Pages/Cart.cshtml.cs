using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Models;

namespace SportsStore.Pages;

public class CartModel : PageModel
{
    private IStoreRepository _repository;

    public CartModel(IStoreRepository repo, Cart cartService)
    {
        _repository = repo;
        Cart = cartService;
    }

    public Cart Cart { get; set; }
    public string ReturnUrl { get; set; } = "/";

    public void OnGet(string returnUrl)
    {
        ReturnUrl = returnUrl ?? "/";
    }

    public IActionResult OnPost(long productId, string returnUrl)
    {
        var product = _repository.Products
            .FirstOrDefault(p => p.ProductID == productId);
        if (product != null) Cart.AddItem(product, 1);
        return RedirectToPage(new {returnUrl});
    }

    public IActionResult OnPostRemove(long productId, string returnUrl)
    {
        Cart.RemoveLine(Cart.Lines.First(cl =>
            cl.Product.ProductID == productId).Product);
        return RedirectToPage(new {returnUrl});
    }
}