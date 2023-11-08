using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using WebApiDynamodbLocal.Constants.BigTimeDeals;
using WebApiDynamodbLocal.Dto.BigTimeDeals.Deal;
using WebApiDynamodbLocal.Entities.BigTimeDeals;
using WebApiDynamodbLocal.Models.BigTimeDeals;
using WebApiDynamodbLocal.Services.BigTimeDeals.Interfaces;

namespace WebApiDynamodbLocal.Controllers.BigTimeDeals;

[Route(ApiPath.Deal)]
[ApiController]
public class DealController : ControllerBase
{
    private readonly IDealService _dealService;

    public DealController(IDealService dealService)
    {
        _dealService = dealService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(string dealId)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(new GetResponseDto
            {
                DealModel = null,
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                Message = "Validation Error",
                Succeeded = false
            });
        var response = await _dealService.GetAsync(dealId);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(DealModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(new ResponseBaseDto
            {
                Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage),
                Message = "Validation Error",
                Succeeded = false
            });
        var deal = new Deal
        {
            Type = nameof(Deal),
            Title = model.Title,
            Link = model.Link,
            Price = model.Price,
            Category = model.Category,
            Brand = model.Brand,
            CreatedAt = DateTime.UtcNow
        };
        var response = await _dealService.CreateAsync(deal);
        if (!response.Succeeded) return UnprocessableEntity(response);
        return CreatedAtAction("Post", new {pk = response.DealId}, response);
    }
}
