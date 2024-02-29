﻿using System.Diagnostics;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiMyBgList.Attributes;
using WebApiMyBgList.Constants;
using WebApiMyBgList.DbContext;
using WebApiMyBgList.Dto;
using WebApiMyBgList.DTO;
using WebApiMyBgList.Models;

namespace WebApiMyBgList.Controllers;

[Route("[controller]")]
[ApiController]
public class DomainsController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet(Name = "GetDomains")]
    [ResponseCache(CacheProfileName = "Any-60")]
    [ManualValidationFilter]
    public async Task<ActionResult<RestDto<Domain[]>>> Get(
        [FromQuery] RequestDto<DomainDto> input)
    {
        if (!ModelState.IsValid)
        {
            var details = new ValidationProblemDetails(ModelState)
            {
                Extensions =
                {
                    ["traceId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                }
            };
            if (ModelState.Keys.Any(k => k == "PageSize"))
            {
                details.Type =
                    "https://tools.ietf.org/html/rfc7231#section-6.6.2";
                details.Status = StatusCodes.Status501NotImplemented;
                return new ObjectResult(details)
                {
                    StatusCode = StatusCodes.Status501NotImplemented
                };
            }

            details.Type =
                "https://tools.ietf.org/html/rfc7231#section-6.5.1";
            details.Status = StatusCodes.Status400BadRequest;
            return new BadRequestObjectResult(details);
        }

        var query = context.Domains.AsQueryable();
        if (!string.IsNullOrEmpty(input.FilterQuery))
            query = query.Where(b => b.Name.Contains(input.FilterQuery));
        var recordCount = await query.CountAsync();
        query = query
            .OrderBy($"{input.SortColumn} {input.SortOrder}")
            .Skip(input.PageIndex * input.PageSize)
            .Take(input.PageSize);

        return new RestDto<Domain[]>
        {
            Data = await query.ToArrayAsync(),
            PageIndex = input.PageIndex,
            PageSize = input.PageSize,
            RecordCount = recordCount,
            Links =
            [
                new LinkDto(
                    Url.Action(null,
                        "Domains",
                        new {input.PageIndex, input.PageSize},
                        Request.Scheme)!,
                    "self",
                    "GET")
            ]
        };
    }

    [Authorize(Roles = RoleNames.Moderator)]
    [HttpPost(Name = "UpdateDomain")]
    [ResponseCache(CacheProfileName = "NoCache")]
    public async Task<RestDto<Domain?>> Post(DomainDto model)
    {
        var domain = await context.Domains
            .Where(b => b.Id == model.Id)
            .FirstOrDefaultAsync();
        if (domain != null)
        {
            if (!string.IsNullOrEmpty(model.Name))
                domain.Name = model.Name;
            domain.LastModifiedDate = DateTime.Now;
            context.Domains.Update(domain);
            await context.SaveChangesAsync();
        }

        return new RestDto<Domain?>
        {
            Data = domain,
            Links =
            [
                new LinkDto(
                    Url.Action(
                        null,
                        "Domains",
                        model,
                        Request.Scheme)!,
                    "self",
                    "POST")
            ]
        };
    }

    [Authorize(Roles = RoleNames.Administrator)]
    [HttpDelete(Name = "DeleteDomain")]
    [ResponseCache(CacheProfileName = "NoCache")]
    public async Task<RestDto<Domain?>> Delete(int id)
    {
        var domain = await context.Domains
            .Where(b => b.Id == id)
            .FirstOrDefaultAsync();
        if (domain != null)
        {
            context.Domains.Remove(domain);
            await context.SaveChangesAsync();
        }

        return new RestDto<Domain?>
        {
            Data = domain,
            Links =
            [
                new LinkDto(
                    Url.Action(null,
                        "Domains",
                        id,
                        Request.Scheme)!,
                    "self",
                    "DELETE")
            ]
        };
    }
}
