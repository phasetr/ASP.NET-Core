using Microsoft.AspNetCore.Mvc;
using WebApiDynamodbLocal.Constants.BigTimeDeals;
using WebApiDynamodbLocal.Dto;
using WebApiDynamodbLocal.Dto.BigTimeDeals.User;
using WebApiDynamodbLocal.Entities.BigTimeDeals;
using WebApiDynamodbLocal.Models.BigTimeDeals;
using WebApiDynamodbLocal.Services.BigTimeDeals.Interfaces;

namespace WebApiDynamodbLocal.Controllers.BigTimeDeals;

[Route(ApiPath.User)]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(string userName)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(new GetResponseDto
            {
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                Message = "Validation Error",
                Succeeded = false
            });
        var response = await userService.GetAsync(userName);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(UserModel model)
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
        var user = new User
        {
            UserName = model.UserName,
            Name = model.Name,
            CreatedAt = DateTime.UtcNow
        };
        var response = await userService.CreateAsync(user);
        if (!response.Succeeded) return UnprocessableEntity(response);
        return CreatedAtAction("Post", new {pk = Key.UserPk(model.UserName)}, response);
    }
}
