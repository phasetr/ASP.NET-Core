using Api.Services;
using Common.Entities;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("rest/[controller]")]
public class BaseController<TModel, TEntity, TService> : ControllerBase
    where TModel : IModel, new()
    where TEntity : BaseEntity
    where TService : BaseService<TEntity, TModel>
{
    private readonly string _createPath;
    private readonly TService _service;

    public BaseController(TService service, string createPath)
    {
        _service = service;
        _createPath = createPath;
    }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] TModel model)
    {
        model = await _service.CreateAsync(model);
        return Created($"{_createPath}/{model.Id}", new {model.Id});
    }

    [HttpPut("{id:int}")]
    public virtual async Task<IActionResult> Update([FromRoute] int id, [FromBody] TModel model)
    {
        await _service.UpdateAsync(id, model);
        return Ok();
    }

    [HttpGet("{id:int}")]
    public virtual async Task<TModel> Get(int id)
    {
        return await _service.GetByIdAsync(id);
    }

    [HttpGet("list")]
    public virtual async Task<IEnumerable<TModel>> GetList()
    {
        var data = await _service.GetAllAsync();
        return data;
    }


    [HttpDelete("{id:int}")]
    public virtual async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
