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
    private readonly ILogger<ApplicationUserController> _logger;

    public ApplicationUserController(
        ApplicationDbContext context,
        ILogger<ApplicationUserController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<ApplicationUser> Get()
    {
        _logger.LogInformation("ApplicationUserController.Get");
        return _context.ApplicationUsers.ToArray();
    }

    [HttpGet("{id}", Name = "Get")]
    public async Task<ApplicationUser?> Get(string id)
    {
        _logger.LogInformation("ApplicationUserController.Get {Id}", id);
        return await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == id);
    }
}
