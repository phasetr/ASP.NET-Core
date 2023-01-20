using System.Security.Claims;
using CityBreaks.AuthorizationRequirements;
using CityBreaks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace CityBreaks.AuthorizationHandlers;

public class PropertyAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Property>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        OperationAuthorizationRequirement requirement, Property resource)
    {
        if (requirement.Name == PropertyOperations.Edit.Name)
            if (resource.CreatorId == context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value)
                context.Succeed(requirement);
        if (requirement.Name != PropertyOperations.Delete.Name) return Task.CompletedTask;
        if (context.User.IsInRole("Admin")) context.Succeed(requirement);
        return Task.CompletedTask;
    }
}