using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.Common.EntityModels.Entities;

namespace BlazorJwtAuth.WebApi.Service.Services.Interfaces;

public interface IApplicationRoleService
{
    Task<ResponseBaseDto> AddRoleToUserAsync(ApplicationUser user, string roleName);
}
