using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApiMyBgList.Constants;
using WebApiMyBgList.DbContext;
using WebApiMyBgList.Dto;
using WebApiMyBgList.Models;

namespace WebApiMyBgList.GraphQL;

public class Mutation
{
    [Serial]
    [Authorize(Roles = new[] {RoleNames.Moderator})]
    public async Task<BoardGame?> UpdateBoardGame(
        [Service] ApplicationDbContext context, BoardGameDto model)
    {
        var boardGame = await context.BoardGames
            .Where(b => b.Id == model.Id)
            .FirstOrDefaultAsync();
        if (boardGame != null)
        {
            if (!string.IsNullOrEmpty(model.Name))
                boardGame.Name = model.Name;
            if (model.Year is > 0)
                boardGame.Year = model.Year.Value;
            boardGame.LastModifiedDate = DateTime.Now;
            context.BoardGames.Update(boardGame);
            await context.SaveChangesAsync();
        }

        return boardGame;
    }

    [Serial]
    [Authorize(Roles = new[] {RoleNames.Administrator})]
    public async Task DeleteBoardGame(
        [Service] ApplicationDbContext context, int id)
    {
        var boardGame = await context.BoardGames
            .Where(b => b.Id == id)
            .FirstOrDefaultAsync();
        if (boardGame != null)
        {
            context.BoardGames.Remove(boardGame);
            await context.SaveChangesAsync();
        }
    }

    [Serial]
    [Authorize(Roles = new[] {RoleNames.Moderator})]
    public async Task<Domain?> UpdateDomain(
        [Service] ApplicationDbContext context, DomainDto model)
    {
        var domain = await context.Domains
            .Where(d => d.Id == model.Id)
            .FirstOrDefaultAsync();
        if (domain != null)
        {
            if (!string.IsNullOrEmpty(model.Name))
                domain.Name = model.Name;
            domain.LastModifiedDate = DateTime.Now;
            context.Domains.Update(domain);
            await context.SaveChangesAsync();
        }

        return domain;
    }

    [Serial]
    [Authorize(Roles = new[] {RoleNames.Administrator})]
    public async Task DeleteDomain(
        [Service] ApplicationDbContext context, int id)
    {
        var domain = await context.Domains
            .Where(d => d.Id == id)
            .FirstOrDefaultAsync();
        if (domain != null)
        {
            context.Domains.Remove(domain);
            await context.SaveChangesAsync();
        }
    }

    [Serial]
    [Authorize(Roles = new[] {RoleNames.Moderator})]
    public async Task<Mechanic?> UpdateMechanic(
        [Service] ApplicationDbContext context, MechanicDto model)
    {
        var mechanic = await context.Mechanics
            .Where(m => m.Id == model.Id)
            .FirstOrDefaultAsync();
        if (mechanic != null)
        {
            if (!string.IsNullOrEmpty(model.Name))
                mechanic.Name = model.Name;
            mechanic.LastModifiedDate = DateTime.Now;

            context.Mechanics.Update(mechanic);
            await context.SaveChangesAsync();
        }

        return mechanic;
    }

    [Serial]
    [Authorize(Roles = new[] {RoleNames.Administrator})]
    public async Task DeleteMechanic(
        [Service] ApplicationDbContext context, int id)
    {
        var mechanic = await context.Mechanics
            .Where(m => m.Id == id)
            .FirstOrDefaultAsync();
        if (mechanic != null)
        {
            context.Mechanics.Remove(mechanic);
            await context.SaveChangesAsync();
        }
    }
}
