using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiMyBgList.Constants;
using WebApiMyBgList.DbContext;
using WebApiMyBgList.Models;
using WebApiMyBgList.Models.Csv;
using Path = System.IO.Path;

namespace WebApiMyBgList.Controllers;

// [Authorize(Roles = RoleNames.Administrator)]
// [ApiExplorerSettings(IgnoreApi = true)]
[Route("[controller]/[action]")]
[ApiController]
public class SeedController(
    ApplicationDbContext context,
    IWebHostEnvironment env,
    RoleManager<IdentityRole> roleManager,
    UserManager<ApiUser> userManager)
    : ControllerBase
{
    [HttpPut]
    [ResponseCache(CacheProfileName = "NoCache")]
    public async Task<IActionResult> BoardGameData()
    {
        // SETUP
        var config = new CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR"))
        {
            HasHeaderRecord = true,
            Delimiter = ";"
        };
        using var reader = new StreamReader(
            Path.Combine(env.ContentRootPath, "Data/bgg_dataset.csv"));
        using var csv = new CsvReader(reader, config);
        var existingBoardGames = await context.BoardGames
            .ToDictionaryAsync(bg => bg.Id);
        var existingDomains = await context.Domains
            .ToDictionaryAsync(d => d.Name);
        var existingMechanics = await context.Mechanics
            .ToDictionaryAsync(m => m.Name);
        var now = DateTime.Now;

        // EXECUTE
        var records = csv.GetRecords<BggRecord>();
        var skippedRows = 0;
        foreach (var record in records)
        {
            if (!record.Id.HasValue
                || string.IsNullOrEmpty(record.Name)
                || existingBoardGames.ContainsKey(record.Id.Value))
            {
                skippedRows++;
                continue;
            }

            var boardGame = new BoardGame
            {
                Id = record.Id.Value,
                Name = record.Name,
                BggRank = record.BggRank ?? 0,
                ComplexityAverage = record.ComplexityAverage ?? 0,
                MaxPlayers = record.MaxPlayers ?? 0,
                MinAge = record.MinAge ?? 0,
                MinPlayers = record.MinPlayers ?? 0,
                OwnedUsers = record.OwnedUsers ?? 0,
                PlayTime = record.PlayTime ?? 0,
                RatingAverage = record.RatingAverage ?? 0,
                UsersRated = record.UsersRated ?? 0,
                Year = record.YearPublished ?? 0,
                CreatedDate = now,
                LastModifiedDate = now
            };
            context.BoardGames.Add(boardGame);

            if (!string.IsNullOrEmpty(record.Domains))
                foreach (var domainName in record.Domains
                             .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                             .Distinct(StringComparer.InvariantCultureIgnoreCase))
                {
                    var domain = existingDomains.GetValueOrDefault(domainName);
                    if (domain == null)
                    {
                        domain = new Domain
                        {
                            Name = domainName,
                            CreatedDate = now,
                            LastModifiedDate = now
                        };
                        context.Domains.Add(domain);
                        existingDomains.Add(domainName, domain);
                    }

                    context.BoardGamesDomains.Add(new BoardGamesDomains
                    {
                        BoardGame = boardGame,
                        Domain = domain,
                        CreatedDate = now
                    });
                }

            if (string.IsNullOrEmpty(record.Mechanics)) continue;
            foreach (var mechanicName in record.Mechanics
                         .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                         .Distinct(StringComparer.InvariantCultureIgnoreCase))
            {
                var mechanic = existingMechanics.GetValueOrDefault(mechanicName);
                if (mechanic == null)
                {
                    mechanic = new Mechanic
                    {
                        Name = mechanicName,
                        CreatedDate = now,
                        LastModifiedDate = now
                    };
                    context.Mechanics.Add(mechanic);
                    existingMechanics.Add(mechanicName, mechanic);
                }

                context.BoardGamesMechanics.Add(new BoardGamesMechanics
                {
                    BoardGame = boardGame,
                    Mechanic = mechanic,
                    CreatedDate = now
                });
            }
        }

        // SAVE
        await context.SaveChangesAsync();
        // SQL Server用の対処
        /*
        await using var transaction = _context.Database.BeginTransaction();
        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT BoardGames ON");
        await _context.SaveChangesAsync();
        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT BoardGames OFF");
        transaction.Commit();
        */

        // RECAP
        return new JsonResult(new
        {
            BoardGames = context.BoardGames.Count(),
            Domains = context.Domains.Count(),
            Mechanics = context.Mechanics.Count(),
            SkippedRows = skippedRows
        });
    }

    [HttpPost]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> AuthData()
    {
        var rolesCreated = 0;
        var usersAddedToRoles = 0;

        if (!await roleManager.RoleExistsAsync(RoleNames.Moderator))
        {
            await roleManager.CreateAsync(
                new IdentityRole(RoleNames.Moderator));
            rolesCreated++;
        }

        if (!await roleManager.RoleExistsAsync(RoleNames.Administrator))
        {
            await roleManager.CreateAsync(
                new IdentityRole(RoleNames.Administrator));
            rolesCreated++;
        }

        var testModerator = await userManager
            .FindByNameAsync("TestModerator");
        if (testModerator != null
            && !await userManager.IsInRoleAsync(
                testModerator, RoleNames.Moderator))
        {
            await userManager.AddToRoleAsync(testModerator, RoleNames.Moderator);
            usersAddedToRoles++;
        }

        var testAdministrator = await userManager
            .FindByNameAsync("TestAdministrator");
        if (testAdministrator == null
            || await userManager.IsInRoleAsync(
                testAdministrator, RoleNames.Administrator))
            return new JsonResult(new
            {
                RolesCreated = rolesCreated,
                UsersAddedToRoles = usersAddedToRoles
            });
        await userManager.AddToRoleAsync(
            testAdministrator, RoleNames.Moderator);
        await userManager.AddToRoleAsync(
            testAdministrator, RoleNames.Administrator);
        usersAddedToRoles++;

        return new JsonResult(new
        {
            RolesCreated = rolesCreated,
            UsersAddedToRoles = usersAddedToRoles
        });
    }
}
