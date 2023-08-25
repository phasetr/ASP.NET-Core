using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlazorJwtAuth.Common.Constants;
using BlazorJwtAuth.Common.EntityModels.Entities;
using BlazorJwtAuth.Common.Models;
using BlazorJwtAuth.Common.Settings;
using JwtAuth.Common.DataContext.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BlazorJwtAuth.WebApi.Service.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly Jwt _jwt;
    private readonly ILogger<UserService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager,
        IOptions<Jwt> jwt,
        ILogger<UserService> logger,
        ApplicationDbContext context)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
        _jwt = jwt.Value;
    }

    public async Task<string> RegisterAsync(RegisterModel model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Username,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };
        var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);

        if (userWithSameEmail != null) return $"Email {user.Email} is already registered.";

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded) await _userManager.AddToRoleAsync(user, Authorization.DefaultRole.ToString());

        return $"User Registered with username {user.UserName}";
    }

    public async Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model)
    {
        var authenticationModel = new AuthenticationModel();
        var user = await _context.Users
            .Include(m => m.RefreshTokens)
            .FirstOrDefaultAsync(m => m.Email == model.Email);
        if (user == null)
        {
            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = $"No Accounts Registered with {model.Email}.";
            _logger.LogInformation("No Accounts Registered with {Email}", model.Email);
            return authenticationModel;
        }

        if (await _userManager.CheckPasswordAsync(user, model.Password))
        {
            authenticationModel.IsAuthenticated = true;
            var jwtSecurityToken = await CreateJwtToken(user);
            authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authenticationModel.Email = user.Email;
            authenticationModel.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            authenticationModel.Roles = rolesList.ToList<string>();

            if (user is {RefreshTokens: not null} && user.RefreshTokens.Any(a => a.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(a => a.IsActive);
                authenticationModel.RefreshToken = activeRefreshToken!.Token;
                authenticationModel.RefreshTokenExpiration = activeRefreshToken.Expires;
            }
            else
            {
                var refreshToken = CreateRefreshToken(user.Id);
                user.RefreshTokens ??= new List<RefreshToken>();
                user.RefreshTokens.Add(refreshToken);
                _context.ApplicationUsers.Update(user);
                await _context.SaveChangesAsync();
                authenticationModel.RefreshToken = refreshToken.Token;
                authenticationModel.RefreshTokenExpiration = refreshToken.Expires;
            }

            authenticationModel.Message = "Token Created Properly.";
            return authenticationModel;
        }

        authenticationModel.IsAuthenticated = false;
        authenticationModel.Message = $"Incorrect Credentials for user {user.Email}.";
        return authenticationModel;
    }

    public async Task<string> AddRoleAsync(AddRoleModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return $"No Accounts Registered with {model.Email}.";
        if (!await _userManager.CheckPasswordAsync(user, model.Password))
            return $"Incorrect Credentials for user {user.Email}.";
        var roleExists = Enum.GetNames(typeof(Authorization.Roles)).Any(x => x.ToLower() == model.Role.ToLower());

        if (!roleExists) return $"Role {model.Role} not found.";
        {
            var validRole = Enum.GetValues(typeof(Authorization.Roles))
                .Cast<Authorization.Roles>().FirstOrDefault(x => x.ToString().ToLower() == model.Role.ToLower());
            await _userManager.AddToRoleAsync(user, validRole.ToString());
            return $"Added {model.Role} to user {model.Email}.";
        }
    }

    public async Task<AuthenticationModel> RefreshTokenAsync(string requestRefreshToken)
    {
        var authenticationModel = new AuthenticationModel();
        var user = _context.Users
            .Include(applicationUser => applicationUser.RefreshTokens)
            .SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == requestRefreshToken));
        if (user == null)
        {
            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = "Token did not match any users.";
            return authenticationModel;
        }

        var oldRefreshToken = user.RefreshTokens.Single(x => x.Token == requestRefreshToken);

        if (!oldRefreshToken.IsActive)
        {
            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = "Token Not Active.";
            return authenticationModel;
        }

        // Revoke Current Refresh Token
        oldRefreshToken.Revoked = DateTime.UtcNow;

        // Generate new Refresh Token and save to Database
        var newRefreshToken = CreateRefreshToken(user.Id);
        user.RefreshTokens.Add(newRefreshToken);
        _context.Update(user);
        await _context.SaveChangesAsync();

        // Generates new jwt
        authenticationModel.IsAuthenticated = true;
        var jwtSecurityToken = await CreateJwtToken(user);
        authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        authenticationModel.Email = user.Email;
        authenticationModel.UserName = user.UserName;
        var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        authenticationModel.Roles = rolesList.ToList<string>();
        authenticationModel.RefreshToken = newRefreshToken.Token;
        authenticationModel.RefreshTokenExpiration = newRefreshToken.Expires;
        authenticationModel.Message = "Token Refreshed Properly.";
        return authenticationModel;
    }

    public async Task<bool> RevokeTokenAsync(string token)
    {
        var user = await _context.Users
            .Include(applicationUser => applicationUser.RefreshTokens)
            .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

        // return false if no user found with token
        if (user == null) return false;

        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        // return false if token is not active
        if (!refreshToken.IsActive) return false;

        // revoke token and save
        refreshToken.Revoked = DateTime.UtcNow;
        _context.Update(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<ApplicationUser?> GetByIdAsync(string id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = roles.Select<string, Claim>(t => new Claim("roles", t)).ToList();

        var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            _jwt.Issuer,
            _jwt.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }

    private static RefreshToken CreateRefreshToken(string userId)
    {
        var randomNumber = new byte[32];
        var rnd = new Random();
        rnd.NextBytes(randomNumber);
        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            ApplicationUserId = userId,
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow
        };
    }

    // TODO : Update User Details
    // TODO : Remove User from Role 
}
