using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicationUserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ApplicationUserController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IEnumerable<ApplicationUser?> Get()
    {
        return _context.ApplicationUsers.ToArray();
    }

    [HttpGet("{id}", Name = "Get")]
    public async Task<ApplicationUser?> Get(string id)
    {
        return await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == id);
    }
}
