using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers;

public class OrderController : Controller
{
    private readonly Cart _cart;
    private readonly IOrderRepository _repository;

    public OrderController(IOrderRepository repoService, Cart cartService)
    {
        _repository = repoService;
        _cart = cartService;
    }

    public ViewResult Checkout()
    {
        return View(new Order());
    }

    [HttpPost]
    public IActionResult Checkout(Order order)
    {
        if (!_cart.Lines.Any()) ModelState.AddModelError("", "Sorry, your cart is empty!");
        if (!ModelState.IsValid) return View();
        order.Lines = _cart.Lines.ToArray();
        _repository.SaveOrder(order);
        _cart.Clear();
        return RedirectToPage("/Completed", new {orderId = order.OrderID});

    }
}