using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcWithApi.Data;
using MvcWithApi.Models;

namespace MvcWithApi.Controllers.Api.V1;

[Route("api/v1/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly MyDbContext _context;

    public TodoItemsController(MyDbContext context)
    {
        _context = context;
    }

    // GET: api/TodoItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodoItems()
    {
        return await _context.TodoItems
            .Select(x => ItemToDto(x))
            .ToListAsync();
    }

    // GET: api/TodoItems/5
    [HttpGet("{id:long}")]
    public async Task<ActionResult<TodoItemDto>> GetTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null) return NotFound();
        return ItemToDto(todoItem);
    }

    // PUT: api/TodoItems/5
    // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id:long}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItemDto todoDto)
    {
        if (id != todoDto.Id) return BadRequest();

        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null) return NotFound();

        todoItem.Name = todoDto.Name;
        todoItem.IsComplete = todoDto.IsComplete;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/TodoItems
    // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TodoItemDto>> PostTodoItem(TodoItemDto todoDto)
    {
        var todoItem = new TodoItem
        {
            IsComplete = todoDto.IsComplete,
            Name = todoDto.Name
        };

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTodoItem),
            new {id = todoItem.Id},
            ItemToDto(todoItem));
    }

    // DELETE: api/TodoItems/5
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null) return NotFound();

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoItemExists(long id)
    {
        // return (_context.TodoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        return _context.TodoItems.Any(e => e.Id == id);
    }

    private static TodoItemDto ItemToDto(TodoItem todoItem)
    {
        return new TodoItemDto
        {
            Id = todoItem.Id,
            Name = todoItem.Name,
            IsComplete = todoItem.IsComplete
        };
    }
}