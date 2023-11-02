using Common.Dto;
using Common.Entities;

namespace Service.Services.Interfaces;

public interface IApplicationRoleService
{
    Task<ResponseBaseDto> AddRoleToUserAsync(ApplicationUser user, string roleName);
}
