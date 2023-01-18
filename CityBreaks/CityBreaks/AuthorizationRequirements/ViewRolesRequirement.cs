using Microsoft.AspNetCore.Authorization;

namespace CityBreaks.AuthorizationRequirements;

public class ViewRolesRequirement : IAuthorizationRequirement
{
    public ViewRolesRequirement(int months)
    {
        Months = months > 0 ? 0 : months;
    }

    public int Months { get; }
}