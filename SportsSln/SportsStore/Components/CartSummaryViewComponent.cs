using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components;

public class CartSummaryViewComponent : ViewComponent
{
    private Cart _cart;

    public CartSummaryViewComponent(Cart cartService)
    {
        _cart = cartService;
    }

    public IViewComponentResult Invoke()
    {
        return View(_cart);
    }
}