using IdentityByRazorPages.Models;
using IdentityByRazorPages.Models.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityByRazorPages.Controllers;

[ApiController]
[Route("/api/people")]
[Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
public class DataController : ControllerBase
{
    private readonly IdContext _context;

    public DataController(IdContext ctx)
    {
        _context = ctx;
    }

    [HttpGet]
    public IEnumerable<Person> GetAll()
    {
        IEnumerable<Person> people
            = _context.People.Include(p => p.Department).Include(p => p.Location);
        foreach (var p in people)
        {
            if (p.Department?.People != null) p.Department.People = null;
            if (p.Location?.People != null) p.Location.People = null;
        }

        return people;
    }


    [HttpGet("{id:long}")]
    public async Task<Person> GetDetails(long id)
    {
        var p = await _context.People.Include(p => p.Department)
            .Include(p => p.Location).FirstAsync(p => p.PersonId == id);
        if (p.Department?.People != null) p.Department.People = null;
        if (p.Location?.People != null) p.Location.People = null;
        return p;
    }

    [HttpPost]
    public async Task Save([FromBody] Person p)
    {
        await _context.People.AddAsync(p);
        await _context.SaveChangesAsync();
    }

    [HttpPut]
    public async Task Update([FromBody] Person p)
    {
        _context.Update(p);
        await _context.SaveChangesAsync();
    }

    [HttpDelete("{id:long}")]
    public async Task Delete(long id)
    {
        _context.People.Remove(new Person {PersonId = id});
        await _context.SaveChangesAsync();
    }

    [HttpGet("/api/locations")]
    public IAsyncEnumerable<Location> GetLocations()
    {
        return _context.Locations.AsAsyncEnumerable();
    }

    [HttpGet("/api/departments")]
    public IAsyncEnumerable<Department> GetDepartments()
    {
        return _context.Departments.AsAsyncEnumerable();
    }
}