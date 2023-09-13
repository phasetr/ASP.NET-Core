using System.Net;
using BlazorJwtAuth.Common.DataContext.Data;
using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.Common.EntityModels.Entities;
using BlazorJwtAuth.WebApi.Service.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace BlazorJwtAuth.WebApi.Service.Services;

public class ApplicationRoleService : IApplicationRoleService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ApplicationRoleService> _logger;

    public ApplicationRoleService(ApplicationDbContext context,
        ILogger<ApplicationRoleService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ResponseBaseDto> AddRoleToUserAsync(ApplicationUser user, string roleName)
    {
        try
        {
            var role = _context.ApplicationRoles.FirstOrDefault(x => x.Name == roleName);
            if (role == null)
                return new ResponseBaseDto
                    {Message = "Role not found.", Status = HttpStatusCode.BadRequest.ToString(), Succeeded = false};
            _context.ApplicationUserRoles.Add(new ApplicationUserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            });
            await _context.SaveChangesAsync();
            return new ResponseBaseDto
                {Message = "Role added.", Status = HttpStatusCode.OK.ToString(), Succeeded = true};
        }
        catch (Exception e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            return new ResponseBaseDto
            {
                Message = e.Message,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Succeeded = false
            };
        }
    }
}
