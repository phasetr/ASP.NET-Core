using System.Linq.Dynamic.Core;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using WebApiMyBgList.Constants;
using WebApiMyBgList.DbContext;
using WebApiMyBgList.Dto;
using WebApiMyBgList.DTO;
using WebApiMyBgList.Extensions;
using WebApiMyBgList.Models;

namespace WebApiMyBgList.Controllers;

[Route("[controller]")]
[ApiController]
public class MechanicsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IDistributedCache _distributedCache;

    public MechanicsController(
        ApplicationDbContext context,
        IDistributedCache distributedCache)
    {
        _context = context;
        _distributedCache = distributedCache;
    }

    [HttpGet(Name = "GetMechanics")]
    [ResponseCache(CacheProfileName = "Any-60")]
    public async Task<RestDto<Mechanic[]>> Get(
        [FromQuery] RequestDto<MechanicDto> input)
    {
        var query = _context.Mechanics.AsQueryable();
        if (!string.IsNullOrEmpty(input.FilterQuery))
            query = query.Where(b => b.Name.Contains(input.FilterQuery));

        var recordCount = await query.CountAsync();

        var cacheKey = $"{input.GetType()}-{JsonSerializer.Serialize(input)}";
        if (!_distributedCache.TryGetValue(cacheKey, out Mechanic[]? result))
        {
            query = query
                .OrderBy($"{input.SortColumn} {input.SortOrder}")
                .Skip(input.PageIndex * input.PageSize)
                .Take(input.PageSize);
            result = await query.ToArrayAsync();
            _distributedCache.Set(cacheKey, result, new TimeSpan(0, 0, 30));
        }

        return new RestDto<Mechanic[]>
        {
            Data = result!,
            PageIndex = input.PageIndex,
            PageSize = input.PageSize,
            RecordCount = recordCount,
            Links = new List<LinkDto>
            {
                new(
                    Url.Action(null,
                        "Mechanics",
                        new {input.PageIndex, input.PageSize},
                        Request.Scheme)!,
                    "self",
                    "GET")
            }
        };
    }

    [Authorize(Roles = RoleNames.Moderator)]
    [HttpPost(Name = "UpdateMechanic")]
    [ResponseCache(CacheProfileName = "NoCache")]
    public async Task<RestDto<Mechanic?>> Post(MechanicDto model)
    {
        var mechanic = await _context.Mechanics
            .Where(b => b.Id == model.Id)
            .FirstOrDefaultAsync();
        if (mechanic != null)
        {
            if (!string.IsNullOrEmpty(model.Name))
                mechanic.Name = model.Name;
            mechanic.LastModifiedDate = DateTime.Now;
            _context.Mechanics.Update(mechanic);
            await _context.SaveChangesAsync();
        }

        return new RestDto<Mechanic?>
        {
            Data = mechanic,
            Links = new List<LinkDto>
            {
                new(
                    Url.Action(
                        null,
                        "Mechanics",
                        model,
                        Request.Scheme)!,
                    "self",
                    "POST")
            }
        };
    }

    [Authorize]
    [HttpDelete(Name = "DeleteMechanic")]
    [ResponseCache(CacheProfileName = "NoCache")]
    public async Task<RestDto<Mechanic?>> Delete(int id)
    {
        var mechanic = await _context.Mechanics
            .Where(b => b.Id == id)
            .FirstOrDefaultAsync();
        if (mechanic != null)
        {
            _context.Mechanics.Remove(mechanic);
            await _context.SaveChangesAsync();
        }

        return new RestDto<Mechanic?>
        {
            Data = mechanic,
            Links = new List<LinkDto>
            {
                new(
                    Url.Action(null,
                        "Mechanics",
                        id,
                        Request.Scheme)!,
                    "self",
                    "DELETE")
            }
        };
    }
}
