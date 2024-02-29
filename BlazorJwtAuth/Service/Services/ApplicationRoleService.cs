using Common.DataContext.Data;
using Common.Dto;
using Common.Entities;
using Microsoft.Extensions.Logging;
using Service.Services.Interfaces;

namespace Service.Services;

public class ApplicationRoleService(
    ApplicationDbContext context,
    ILogger<ApplicationRoleService> logger)
    : IApplicationRoleService
{
    public async Task<ResponseBaseDto> AddRoleToUserAsync(ApplicationUser user, string roleName)
    {
        try
        {
            var role = context.ApplicationRoles.FirstOrDefault(x => x.Name == roleName);
            if (role == null)
                return new ResponseBaseDto
                    {Message = "Role not found.", Succeeded = false};
            context.ApplicationUserRoles.Add(new ApplicationUserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            });
            await context.SaveChangesAsync();
            return new ResponseBaseDto
                {Message = "Role added.", Succeeded = true};
        }
        catch (Exception e)
        {
            logger.LogError("{E}", e.Message);
            logger.LogError("{E}", e.StackTrace);
            return new ResponseBaseDto
            {
                Message = e.Message,
                Succeeded = false
            };
        }
    }
}
