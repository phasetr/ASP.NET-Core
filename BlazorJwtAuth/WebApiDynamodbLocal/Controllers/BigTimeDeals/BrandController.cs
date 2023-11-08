using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using WebApiDynamodbLocal.Constants.BigTimeDeals;
using WebApiDynamodbLocal.Dto.BigTimeDeals.Brand;
using WebApiDynamodbLocal.Entities.BigTimeDeals;
using WebApiDynamodbLocal.Models.BigTimeDeals;
using WebApiDynamodbLocal.Services.BigTimeDeals.Interfaces;

namespace WebApiDynamodbLocal.Controllers.BigTimeDeals;

[Route(ApiPath.Brand)]
[ApiController]
public class BrandController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(string name)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(new GetResponseDto
            {
                BrandModel = null,
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                Message = "Validation Error",
                Succeeded = false
            });
        var response = await _brandService.GetAsync(name);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(BrandModel brandModel)
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
        var brand = new Brand
        {
            Type = nameof(Brand),
            Name = brandModel.Name,
            LogoUrl = brandModel.LogoUrl,
            LikeCount = brandModel.LikeCount,
            WatchCount = brandModel.WatchCount
        };
        var response = await _brandService.CreateAsync(brand);
        if (!response.Succeeded) return UnprocessableEntity(response);
        return CreatedAtAction("Post", new {pk = Brand.ToPk(brandModel.Name)}, response);
    }
}
