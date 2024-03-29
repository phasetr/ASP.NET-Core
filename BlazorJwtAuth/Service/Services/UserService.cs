﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Constants;
using Common.DataContext.Data;
using Common.Dto;
using Common.Entities;
using Common.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Services.Interfaces;

namespace Service.Services;

public class UserService(
    UserManager<ApplicationUser> userManager,
    IOptions<Jwt> jwt,
    ILogger<UserService> logger,
    ApplicationDbContext context,
    IApplicationRoleService applicationRoleService)
    : IUserService
{
    private readonly Jwt _jwt = jwt.Value;

    public async Task<UserRegisterResponseDto> RegisterAsync(UserRegisterDto dto)
    {
        try
        {
            var user = new ApplicationUser
            {
                UserName = dto.Username ?? "",
                Email = dto.Email,
                FirstName = dto.FirstName ?? "",
                LastName = dto.LastName ?? ""
            };
            var result = await userManager.CreateAsync(user, dto.Password ?? string.Empty);
            if (!result.Succeeded)
                return new UserRegisterResponseDto
                {
                    Message = "User Registration Failed.",
                    Succeeded = false,
                    Errors = result.Errors.Select(x => x.Description ?? "")
                };
            var roleResult =
                await applicationRoleService.AddRoleToUserAsync(user, Authorization.DefaultRole.ToString());
            if (!roleResult.Succeeded)
                return new UserRegisterResponseDto
                {
                    Message = "User Registration Failed.",
                    Succeeded = false,
                    Errors = roleResult.Errors.Select(x => x.ToString())
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
            logger.LogError("{E}", e.Message);
            logger.LogError("{E}", e.StackTrace);
            return new UserRegisterResponseDto
            {
                Errors = new List<string> {e.Message},
                Message = e.Message,
                Succeeded = false
            };
        }
    }

    public async Task<AuthenticationResponseDto> GetTokenAsync(GetTokenDto model)
    {
        var authResponseDto = new AuthenticationResponseDto();
        var user = await context.Users
            .Include(m => m.RefreshTokens)
            .FirstOrDefaultAsync(m => m.Email == model.Email);
        if (user == null)
        {
            authResponseDto.IsAuthenticated = false;
            authResponseDto.Message = $"No Accounts Registered with {model.Email}.";
            logger.LogInformation("No Accounts Registered with {Email}", model.Email);
            return authResponseDto;
        }

        if (await userManager.CheckPasswordAsync(user, model.Password))
        {
            authResponseDto.IsAuthenticated = true;
            var jwtSecurityToken = await CreateJwtToken(user);
            authResponseDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authResponseDto.Email = user.Email ?? string.Empty;
            authResponseDto.UserName = user.UserName ?? string.Empty;
            var rolesList = await userManager.GetRolesAsync(user).ConfigureAwait(false);
            authResponseDto.Roles = rolesList.ToList();

            if (user is {RefreshTokens: not null} && user.RefreshTokens.Any(a => a.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(a => a.IsActive);
                authResponseDto.RefreshToken = activeRefreshToken!.Token;
                authResponseDto.RefreshTokenExpiration = activeRefreshToken.Expires;
            }
            else
            {
                var refreshToken = CreateRefreshToken(user.Id);
                user.RefreshTokens ??= new List<RefreshToken>();
                user.RefreshTokens.Add(refreshToken);
                context.ApplicationUsers.Update(user);
                await context.SaveChangesAsync();
                authResponseDto.RefreshToken = refreshToken.Token;
                authResponseDto.RefreshTokenExpiration = refreshToken.Expires;
            }

            authResponseDto.Message = "Token Created Properly.";
            return authResponseDto;
        }

        authResponseDto.IsAuthenticated = false;
        authResponseDto.Message = $"Incorrect Credentials for user {user.Email}.";
        return authResponseDto;
    }

    public async Task<string> AddRoleAsync(AddRoleDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user == null) return $"No Accounts Registered with {dto.Email}.";
        var roleExists = Enum.GetNames(typeof(Authorization.Roles)).Any(x => x.ToLower().Equals(dto.Role.ToLower()));

        if (!roleExists) return $"Role {dto.Role} not found.";
        {
            var validRole = Enum.GetValues(typeof(Authorization.Roles))
                .Cast<Authorization.Roles>().FirstOrDefault(x => x.ToString().ToLower() == dto.Role.ToLower());
            await userManager.AddToRoleAsync(user, validRole.ToString());
            return $"Added {dto.Role} to user {dto.Email}.";
        }
    }

    public async Task<AuthenticationResponseDto> RefreshTokenAsync(string requestRefreshToken)
    {
        var authResponseDto = new AuthenticationResponseDto();
        var user = context.Users
            .Include(applicationUser => applicationUser.RefreshTokens)
            .SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == requestRefreshToken));
        if (user == null)
        {
            authResponseDto.IsAuthenticated = false;
            authResponseDto.Message = "Token did not match any users.";
            return authResponseDto;
        }

        var oldRefreshToken = user.RefreshTokens.Single(x => x.Token == requestRefreshToken);

        if (!oldRefreshToken.IsActive)
        {
            authResponseDto.IsAuthenticated = false;
            authResponseDto.Message = "Token Not Active.";
            return authResponseDto;
        }

        // Revoke Current Refresh Token
        oldRefreshToken.Revoked = DateTime.UtcNow;

        // Generate new Refresh Token and save to Database
        var newRefreshToken = CreateRefreshToken(user.Id);
        user.RefreshTokens.Add(newRefreshToken);
        context.Update(user);
        await context.SaveChangesAsync();

        // Generates new jwt
        authResponseDto.IsAuthenticated = true;
        var jwtSecurityToken = await CreateJwtToken(user);
        authResponseDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        authResponseDto.Email = user.Email ?? string.Empty;
        authResponseDto.UserName = user.UserName ?? string.Empty;
        var rolesList = await userManager.GetRolesAsync(user).ConfigureAwait(false);
        authResponseDto.Roles = rolesList.ToList();
        authResponseDto.RefreshToken = newRefreshToken.Token;
        authResponseDto.RefreshTokenExpiration = newRefreshToken.Expires;
        authResponseDto.Message = "Token Refreshed Properly.";
        return authResponseDto;
    }

    public async Task<bool> RevokeTokenAsync(string token)
    {
        var user = await context.Users
            .Include(applicationUser => applicationUser.RefreshTokens)
            .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

        // return false if no user found with token
        if (user == null) return false;

        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        // return false if token is not active
        if (!refreshToken.IsActive) return false;

        // revoke token and save
        refreshToken.Revoked = DateTime.UtcNow;
        context.Update(user);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string email)
    {
        // return await _userManager.FindByEmailAsync(email);
        return await context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        var userClaims = await userManager.GetClaimsAsync(user);
        var roles = await userManager.GetRolesAsync(user);

        var roleClaims = roles.Select<string, Claim>(t => new Claim("roles", t)).ToList();

        var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
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
