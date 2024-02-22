using Microsoft.AspNetCore.Mvc;
using WebApiDynamodbLocal.Constants.BigTimeDeals;
using WebApiDynamodbLocal.Dto;
using WebApiDynamodbLocal.Dto.BigTimeDeals.Category;
using WebApiDynamodbLocal.Entities.BigTimeDeals;
using WebApiDynamodbLocal.Services.BigTimeDeals.Interfaces;

namespace WebApiDynamodbLocal.Controllers.BigTimeDeals;

[Route(ApiPath.Category)]
[ApiController]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(string name)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(new GetResponseDto
            {
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                Message = "Validation Error",
                Succeeded = false
            });
        var response = await categoryService.GetAsync(name);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(PostCategoryDto dto)
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
        var category = new Category
        {
            Name = dto.Name,
            FeaturedDeals = dto.FeaturedDeals,
            LikeCount = 0,
            WatchCount = 0
        };
        var response = await categoryService.CreateAsync(category);
        if (!response.Succeeded) return UnprocessableEntity(response);
        return CreatedAtAction("Post", new {pk = Key.CategoryPk(dto.Name)}, response);
    }
}
