using MediaLibrary.Api.Services;
using MediaLibrary.Common.Entities;
using MediaLibrary.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace MediaLibrary.Api.Controllers;

[ApiController]
[Route("rest/[controller]")]
public class BaseController<TModel, TEntity, TService>(TService service, string createPath) : ControllerBase
    where TModel : IModel, new()
    where TEntity : BaseEntity
    where TService : BaseService<TEntity, TModel>
{
    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] TModel model)
    {
        model = await service.CreateAsync(model);
        return Created($"{createPath}/{model.Id}", new {model.Id});
    }

    [HttpPut("{id:int}")]
    public virtual async Task<IActionResult> Update([FromRoute] int id, [FromBody] TModel model)
    {
        await service.UpdateAsync(id, model);
        return Ok();
    }

    [HttpGet("{id:int}")]
    public virtual async Task<TModel> Get(int id)
    {
        return await service.GetByIdAsync(id);
    }

    [HttpGet("list")]
    public virtual async Task<IEnumerable<TModel>> GetList()
    {
        var data = await service.GetAllAsync();
        return data;
    }


    [HttpDelete("{id:int}")]
    public virtual async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}
