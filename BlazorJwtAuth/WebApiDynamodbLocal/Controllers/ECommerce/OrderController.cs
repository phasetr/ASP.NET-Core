using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using WebApiDynamodbLocal.Constants.ECommerce;
using WebApiDynamodbLocal.Dto.ECommerce.Order;
using WebApiDynamodbLocal.Entities.ECommerce;
using WebApiDynamodbLocal.Services.ECommerce.Interfaces;

namespace WebApiDynamodbLocal.Controllers.ECommerce;

[Route(ApiPath.Order)]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(string orderId)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(new GetOrderDto
            {
                Order = null,
                Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage),
                Message = "Validation Error",
                Succeeded = false
            });
        var response = await _orderService.GetByOrderIdAsync(orderId);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(PostOrderDto postOrderDto)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(new ResponseBaseDto
            {
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                Message = "Validation Error",
                Succeeded = false
            });
        var dateTime = DateTime.UtcNow;
        var response = await _orderService.CreateAsync(postOrderDto);
        if (!response.Succeeded) return UnprocessableEntity(response);
        return CreatedAtAction("Post", new {pk = Order.GenerateOrderId(dateTime)}, response);
    }
}
