using System.Linq.Dynamic.Core;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.Annotations;
using WebApiMyBgList.Attributes;
using WebApiMyBgList.Constants;
using WebApiMyBgList.DbContext;
using WebApiMyBgList.Dto;
using WebApiMyBgList.DTO;
using WebApiMyBgList.Models;

namespace WebApiMyBgList.Controllers;

[Route("[controller]")]
[ApiController]
public class BoardGamesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<BoardGamesController> _logger;

    private readonly IMemoryCache _memoryCache;

    public BoardGamesController(
        ApplicationDbContext context,
        ILogger<BoardGamesController> logger,
        IMemoryCache memoryCache)
    {
        _context = context;
        _logger = logger;
        _memoryCache = memoryCache;
    }

    [HttpGet(Name = "GetBoardGames")]
    [ResponseCache(CacheProfileName = "Any-60")]
    [SwaggerOperation(
        Summary = "Get a list of board games.",
        Description = "Retrieves a list of board games " +
                      "with custom paging, sorting, and filtering rules.")]
    public async Task<RestDto<BoardGame[]>> Get(
        [FromQuery]
        [SwaggerParameter("A DTO object that can be used " +
                          "to customize some retrieval parameters.")]
        RequestDto<BoardGameDto> input)
    {
        _logger.LogInformation(CustomLogEvents.BoardGamesControllerGet,
            "Get method started");

        var query = _context.BoardGames.AsQueryable();
        if (!string.IsNullOrEmpty(input.FilterQuery))
            query = query.Where(b => b.Name.Contains(input.FilterQuery));

        var recordCount = await query.CountAsync();

        var cacheKey = $"{input.GetType()}-{JsonSerializer.Serialize(input)}";
        if (!_memoryCache.TryGetValue<BoardGame[]>(cacheKey, out var result))
        {
            query = query
                .OrderBy($"{input.SortColumn} {input.SortOrder}")
                .Skip(input.PageIndex * input.PageSize)
                .Take(input.PageSize);
            result = await query.ToArrayAsync();
            _memoryCache.Set(cacheKey, result, new TimeSpan(0, 0, 30));
        }

        return new RestDto<BoardGame[]>
        {
            Data = result,
            PageIndex = input.PageIndex,
            PageSize = input.PageSize,
            RecordCount = recordCount,
            Links = new List<LinkDto>
            {
                new(
                    Url.Action(
                        null,
                        "BoardGames",
                        new {input.PageIndex, input.PageSize},
                        Request.Scheme)!,
                    "self",
                    "GET")
            }
        };
    }

    [HttpGet("{id}")]
    [ResponseCache(CacheProfileName = "Any-60")]
    [SwaggerOperation(
        Summary = "Get a single board game.",
        Description = "Retrieves a single board game with the given Id.")]
    public async Task<RestDto<BoardGame?>> Get(
        [CustomKeyValue("x-test-3", "value 3")]
        int id
    )
    {
        _logger.LogInformation(CustomLogEvents.BoardGamesControllerGet,
            "GetBoardGame method started");

        var cacheKey = $"GetBoardGame-{id}";
        if (_memoryCache.TryGetValue<BoardGame>(cacheKey, out var result))
            return new RestDto<BoardGame?>
            {
                Data = result,
                PageIndex = 0,
                PageSize = 1,
                RecordCount = result != null ? 1 : 0,
                Links = new List<LinkDto>
                {
                    new(
                        Url.Action(
                            null,
                            "BoardGames",
                            new {id},
                            Request.Scheme)!,
                        "self",
                        "GET")
                }
            };
        result = await _context.BoardGames.FirstOrDefaultAsync(bg => bg.Id == id);
        _memoryCache.Set(cacheKey, result, new TimeSpan(0, 0, 30));

        return new RestDto<BoardGame?>
        {
            Data = result,
            PageIndex = 0,
            PageSize = 1,
            RecordCount = result != null ? 1 : 0,
            Links = new List<LinkDto>
            {
                new(
                    Url.Action(
                        null,
                        "BoardGames",
                        new {id},
                        Request.Scheme)!,
                    "self",
                    "GET")
            }
        };
    }

    [Authorize(Roles = RoleNames.Moderator)]
    [HttpPost(Name = "UpdateBoardGame")]
    [ResponseCache(CacheProfileName = "NoCache")]
    [SwaggerOperation(
        Summary = "Updates a board game.",
        Description = "Updates the board game's data.")]
    public async Task<RestDto<BoardGame?>> Post(BoardGameDto model)
    {
        var boardGame = await _context.BoardGames
            .Where(b => b.Id == model.Id)
            .FirstOrDefaultAsync();
        if (boardGame == null)
            return new RestDto<BoardGame?>
            {
                Data = boardGame,
                Links = new List<LinkDto>
                {
                    new(
                        Url.Action(
                            null,
                            "BoardGames",
                            model,
                            Request.Scheme)!,
                        "self",
                        "POST")
                }
            };
        if (!string.IsNullOrEmpty(model.Name))
            boardGame.Name = model.Name;
        if (model.Year is > 0)
            boardGame.Year = model.Year.Value;
        boardGame.LastModifiedDate = DateTime.Now;
        _context.BoardGames.Update(boardGame);
        await _context.SaveChangesAsync();

        return new RestDto<BoardGame?>
        {
            Data = boardGame,
            Links = new List<LinkDto>
            {
                new(
                    Url.Action(
                        null,
                        "BoardGames",
                        model,
                        Request.Scheme)!,
                    "self",
                    "POST")
            }
        };
    }

    [Authorize(Roles = RoleNames.Administrator)]
    [HttpDelete(Name = "DeleteBoardGame")]
    [ResponseCache(CacheProfileName = "NoCache")]
    [SwaggerOperation(
        Summary = "Deletes a board game.",
        Description = "Deletes a board game from the database.")]
    public async Task<RestDto<BoardGame?>> Delete(int id)
    {
        var boardGame = await _context.BoardGames
            .Where(b => b.Id == id)
            .FirstOrDefaultAsync();
        if (boardGame == null)
            return new RestDto<BoardGame?>
            {
                Data = boardGame,
                Links = new List<LinkDto>
                {
                    new(
                        Url.Action(
                            null,
                            "BoardGames",
                            id,
                            Request.Scheme)!,
                        "self",
                        "DELETE")
                }
            };
        _context.BoardGames.Remove(boardGame);
        await _context.SaveChangesAsync();

        return new RestDto<BoardGame?>
        {
            Data = boardGame,
            Links = new List<LinkDto>
            {
                new(
                    Url.Action(
                        null,
                        "BoardGames",
                        id,
                        Request.Scheme)!,
                    "self",
                    "DELETE")
            }
        };
    }
}
