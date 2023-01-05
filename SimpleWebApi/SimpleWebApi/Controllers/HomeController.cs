using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleWebApi.Models;

namespace SimpleWebApi.Controllers;

[ApiController]
[Route("/")]
public class HomeController
{
    private readonly TodoContext _context;

    public HomeController(TodoContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> Index()
    {
        return await _context.TodoItems.ToListAsync();
    }
}