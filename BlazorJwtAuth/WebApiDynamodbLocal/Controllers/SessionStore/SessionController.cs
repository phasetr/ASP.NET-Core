using Microsoft.AspNetCore.Mvc;
using WebApiDynamodbLocal.Constants.SessionStore;
using WebApiDynamodbLocal.Dto;
using WebApiDynamodbLocal.Dto.SessionStore;
using WebApiDynamodbLocal.Services.SessionStore.interfaces;

namespace WebApiDynamodbLocal.Controllers.SessionStore;

[Route(ApiPath.Session)]
[ApiController]
public class SessionController(ISessionService sessionService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(string sessionId)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(new GetResponseDto
            {
                UserName = string.Empty,
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                Message = "Validation Error",
                Succeeded = false
            });
        var response = await sessionService.GetAsync(sessionId);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(PostDto dto)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(new ResponseBaseDto
            {
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                Message = "Validation Error",
                Succeeded = false
            });
        var response = await sessionService.CreateAsync(dto);
        if (!response.Succeeded) return UnprocessableEntity(response);
        return CreatedAtAction("Post", new {pk = response.Key}, response);
    }

    [HttpDelete(ApiPath.SessionUser)]
    public async Task<IActionResult> DeleteByUserNameAsync(string userName)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(new PostResponseDto
            {
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                Message = "Validation Error",
                Succeeded = false
            });
        var response = await sessionService.DeleteByUserNameAsync(userName);
        if (!response.Succeeded) return UnprocessableEntity(response);
        return Ok(response);
    }
}
