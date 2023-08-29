using System.Security.Claims;
using BlazorJwtAuth.Common.EntityModels.Entities;

namespace BlazorJwtAuth.WebApi.Service.Services;

public interface IClaimsService
{
    Task<List<Claim>> GetUserClaimsAsync(ApplicationUser user);   
}
