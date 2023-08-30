using System.Security.Claims;
using BlazorJwtAuth.Common.EntityModels.Entities;

namespace BlazorJwtAuth.WebApi.Service.Services.Interfaces;

public interface IClaimsService
{
    Task<List<Claim>> GetUserClaimsAsync(ApplicationUser user);   
}
