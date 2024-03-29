using Microsoft.AspNetCore.Mvc;
using WebApiDynamodbLocal.Constants.BigTimeDeals;
using WebApiDynamodbLocal.Dto;
using WebApiDynamodbLocal.Dto.BigTimeDeals.Brand;
using WebApiDynamodbLocal.Entities.BigTimeDeals;
using WebApiDynamodbLocal.Models.BigTimeDeals;
using WebApiDynamodbLocal.Services.BigTimeDeals.Interfaces;

namespace WebApiDynamodbLocal.Controllers.BigTimeDeals;

[Route(ApiPath.Brand)]
[ApiController]
public class BrandController(IBrandService brandService) : ControllerBase
{
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
        var response = await brandService.GetAsync(name);
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
            Name = brandModel.Name,
            LogoUrl = brandModel.LogoUrl,
            LikeCount = brandModel.LikeCount,
            WatchCount = brandModel.WatchCount
        };
        var response = await brandService.CreateAsync(brand);
        if (!response.Succeeded) return UnprocessableEntity(response);
        return CreatedAtAction("Post", new {pk = Key.BrandPk(brandModel.Name)}, response);
    }
}
