using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using WebApi.Data;
using WebApi.Models;
using WebApi.Models.Authentication;
using WebApi.Services.Authorization;

namespace WebApi.Services;

public interface IApplicationUserService
{
    Response Authenticate(Request model, string ipAddress);
    Response RefreshToken(string token, string ipAddress);
    void RevokeToken(string token, string ipAddress);
    IEnumerable<ApplicationUser> GetAll();
    ApplicationUser GetById(string id);
}

public class ApplicationUserService : IApplicationUserService
{
    private readonly AppSettings _appSettings;
    private readonly ApplicationDbContext _context;
    private readonly IJwtUtils _jwtUtils;
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationUserService(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IJwtUtils jwtUtils,
        IOptions<AppSettings> appSettings)
    {
        _context = context;
        _userManager = userManager;
        _jwtUtils = jwtUtils;
        _appSettings = appSettings.Value;
    }

    public Response Authenticate(Request model, string ipAddress)
    {
        var user = _context.ApplicationUsers.SingleOrDefault(x => x.UserName == model.UserName);
        // validate
        if (user is null) throw new AppException("UserName or password is incorrect");
        var passwordValidator = new PasswordValidator<ApplicationUser>();
        var result = passwordValidator.ValidateAsync(_userManager, user, model.Password).Result;
        if (!result.Succeeded) throw new AppException("UserName or password is incorrect");

        // authentication successful so generate jwt and refresh tokens
        var jwtToken = _jwtUtils.GenerateJwtToken(user);
        var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
        user.RefreshTokens.Add(refreshToken);

        // remove old refresh tokens from applicationUser
        RemoveOldRefreshTokens(user);

        // save changes to db
        _context.Update(user);
        _context.SaveChanges();

        return new Response(user, jwtToken, refreshToken.Token);
    }

    public Response RefreshToken(string token, string ipAddress)
    {
        var user = GetUserByRefreshToken(token);
        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (refreshToken.IsRevoked)
        {
            // revoke all descendant tokens in case this token has been compromised
            RevokeDescendantRefreshTokens(refreshToken, user, ipAddress,
                $"Attempted reuse of revoked ancestor token: {token}");
            _context.Update(user);
            _context.SaveChanges();
        }

        if (!refreshToken.IsActive)
            throw new AppException("Invalid token");

        // replace old refresh token with a new one (rotate token)
        var newRefreshToken = RotateRefreshToken(refreshToken, ipAddress);
        user.RefreshTokens.Add(newRefreshToken);

        // remove old refresh tokens from applicationUser
        RemoveOldRefreshTokens(user);

        // save changes to db
        _context.Update(user);
        _context.SaveChanges();

        // generate new jwt
        var jwtToken = _jwtUtils.GenerateJwtToken(user);

        return new Response(user, jwtToken, newRefreshToken.Token);
    }

    public void RevokeToken(string token, string ipAddress)
    {
        var user = GetUserByRefreshToken(token);
        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (!refreshToken.IsActive)
            throw new AppException("Invalid token");

        // revoke token and save
        RevokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
        _context.Update(user);
        _context.SaveChanges();
    }

    public IEnumerable<ApplicationUser> GetAll()
    {
        return _context.ApplicationUsers;
    }

    public ApplicationUser GetById(string id)
    {
        var user = _context.ApplicationUsers.Find(id);
        if (user == null) throw new KeyNotFoundException($"{nameof(ApplicationUser)} not found");
        return user;
    }

    // helper methods

    private ApplicationUser GetUserByRefreshToken(string token)
    {
        var user = _context.ApplicationUsers.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

        if (user == null)
            throw new AppException("Invalid token");

        return user;
    }

    private RefreshToken RotateRefreshToken(RefreshToken refreshToken, string ipAddress)
    {
        var newRefreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
        RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }

    private void RemoveOldRefreshTokens(ApplicationUser applicationUser)
    {
        // remove old inactive refresh tokens from applicationUser based on TTL in app settings
        applicationUser.RefreshTokens.RemoveAll(x =>
            !x.IsActive &&
            x.Created.AddDays(_appSettings.RefreshTokenTtl) <= DateTime.UtcNow);
    }

    private void RevokeDescendantRefreshTokens(RefreshToken refreshToken, ApplicationUser applicationUser, string ipAddress,
        string reason)
    {
        // recursively traverse the refresh token chain and ensure all descendants are revoked
        if (string.IsNullOrEmpty(refreshToken.ReplacedByToken)) return;
        var childToken = applicationUser.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
        if (childToken is {IsActive: true})
            RevokeRefreshToken(childToken, ipAddress, reason);
        else
            RevokeDescendantRefreshTokens(childToken, applicationUser, ipAddress, reason);
    }

    private static void RevokeRefreshToken(RefreshToken token, string ipAddress, string reason = null,
        string replacedByToken = null)
    {
        token.Revoked = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;
        token.ReasonRevoked = reason;
        token.ReplacedByToken = replacedByToken;
    }
}
