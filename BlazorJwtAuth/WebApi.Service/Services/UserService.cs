using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Constants;
using Common.DataContext.Data;
using Common.Dto;
using Common.EntityModels.Entities;
using Common.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Service.Services.Interfaces;

namespace WebApi.Service.Services;

public class UserService : IUserService
{
    private readonly IApplicationRoleService _applicationRoleService;
    private readonly ApplicationDbContext _context;
    private readonly Jwt _jwt;
    private readonly ILogger<UserService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager,
        IOptions<Jwt> jwt,
        ILogger<UserService> logger,
        ApplicationDbContext context,
        IApplicationRoleService applicationRoleService)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
        _jwt = jwt.Value;
        _applicationRoleService = applicationRoleService;
    }

    public async Task<UserRegisterResponseDto> RegisterAsync(UserRegisterDto dto)
    {
        try
        {
            var user = new ApplicationUser
            {
                UserName = dto.Username ?? "",
                Email = dto.Email,
                FirstName = dto.FirstName ?? "",
                LastName = dto.LastName ?? "",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(dto.Email);

            if (userWithSameEmail != null)
                return new UserRegisterResponseDto
                {
                    Message = $"Email {user.Email} is already registered.",
                    Succeeded = false,
                    Errors = new List<string>
                    {
                        $"Email {user.Email} is already registered."
                    }
                };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return new UserRegisterResponseDto
                {
                    Message = "User Registration Failed.",
                    Succeeded = false,
                    Errors = result.Errors.Select(x => x.ToString() ?? "")
                };
            var roleResult =
                await _applicationRoleService.AddRoleToUserAsync(user, Authorization.DefaultRole.ToString());
            if (!roleResult.Succeeded)
                return new UserRegisterResponseDto
                {
                    Message = "User Registration Failed.",
                    Succeeded = false,
                    Errors = new List<string> {roleResult.Message}
                };
            return new UserRegisterResponseDto
            {
                Message = $"User Registered with username {user.UserName}",
                Succeeded = true,
                Errors = new List<string>()
            };
        }
        catch (Exception e)
        {
            _logger.LogError("{E}", e.Message);
            _logger.LogError("{E}", e.StackTrace);
            return new UserRegisterResponseDto
            {
                Errors = new List<string> {"Some error occurs in the server."},
                Message = "Some error occurs in the server.",
                Succeeded = false
            };
        }
    }

    public async Task<AuthenticationResponseDto> GetTokenAsync(GetTokenDto model)
    {
        var authenticationModel = new AuthenticationResponseDto();
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

    public async Task<string> AddRoleAsync(AddRoleDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null) return $"No Accounts Registered with {dto.Email}.";
        if (!await _userManager.CheckPasswordAsync(user, dto.Password))
            return $"Incorrect Credentials for user {user.Email}.";
        var roleExists = Enum.GetNames(typeof(Authorization.Roles)).Any(x => x.ToLower() == dto.Role.ToLower());

        if (!roleExists) return $"Role {dto.Role} not found.";
        {
            var validRole = Enum.GetValues(typeof(Authorization.Roles))
                .Cast<Authorization.Roles>().FirstOrDefault(x => x.ToString().ToLower() == dto.Role.ToLower());
            await _userManager.AddToRoleAsync(user, validRole.ToString());
            return $"Added {dto.Role} to user {dto.Email}.";
        }
    }

    public async Task<AuthenticationResponseDto> RefreshTokenAsync(string requestRefreshToken)
    {
        var authenticationModel = new AuthenticationResponseDto();
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

    public async Task<ApplicationUser?> FindByEmailAsync(string email)
    {
        // return await _userManager.FindByEmailAsync(email);
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
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
