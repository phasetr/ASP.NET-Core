using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BlazorJwtAuth.Common.Constants;
using BlazorJwtAuth.Common.Dto;
using BlazorJwtAuth.Common.EntityModels.Entities;
using BlazorJwtAuth.Common.Settings;
using BlazorJwtAuth.Test.Unit.FakesSqlite;
using BlazorJwtAuth.WebApi.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace BlazorJwtAuth.Test.Unit.WebApi.Service.Service;

public partial class UserServiceTests : SqliteMemoryBase
{
    private readonly IOptions<Jwt> _jwt = new OptionsWrapper<Jwt>(new Jwt
    {
        Audience = "SecureApiUser",
        DurationInMinutes = 1,
        Issuer = "SecureApi",
        Key = "C1CF4B7DC4C4175B6618DE4F55CA4as#"
    });

    private UserService _sut = default!;

    [Fact]
    public async Task GetTokenAsync_NoUser_NotAuthenticated()
    {
        var context = new ApplicationDbContextFakeSqliteBuilder().Build();
        var userStore = new UserStore<ApplicationUser>(context);
        var userManager = new UserManager<ApplicationUser>(userStore, null, null, null, null, null, null, null, null);
        var mockLogger = Substitute.For<ILogger<UserService>>();
        var mockApplicationRoleLogger = Substitute.For<ILogger<ApplicationRoleService>>();
        var applicationRoleService = new ApplicationRoleService(context, mockApplicationRoleLogger);
        _sut = new UserService(userManager, _jwt, mockLogger, context, applicationRoleService);

        var tokenRequestModel = new GetTokenResponseDto
        {
            Email = "noUser@phasetr.com",
            Password = "noUser"
        };
        var token = await _sut.GetTokenAsync(tokenRequestModel);

        Assert.False(token.IsAuthenticated);
        Assert.Equal($"No Accounts Registered with {tokenRequestModel.Email}.", token.Message);
    }

    [Fact]
    public async Task GetTokenAsync_ExistingUserWrongPassword_IncorrectCredential()
    {
        var tokenRequestModel = new GetTokenResponseDto
        {
            Email = "user@secureapi.com",
            Password = "errorPassword"
        };

        var context = new ApplicationDbContextFakeSqliteBuilder().Build();
        var userStore = new UserStore<ApplicationUser>(context);
        var mockUserManager =
            Substitute.For<UserManager<ApplicationUser>>(userStore, null, null, null, null, null, null, null, null);
        mockUserManager.FindByEmailAsync(Arg.Any<string>()).Returns(new ApplicationUser
        {
            Email = tokenRequestModel.Email
        });
        mockUserManager.CheckPasswordAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(false);
        var mockLogger = Substitute.For<ILogger<UserService>>();
        var mockApplicationRoleLogger = Substitute.For<ILogger<ApplicationRoleService>>();
        var applicationRoleService = new ApplicationRoleService(context, mockApplicationRoleLogger);
        _sut = new UserService(mockUserManager, _jwt, mockLogger, context, applicationRoleService);

        var token = await _sut.GetTokenAsync(tokenRequestModel);

        Assert.False(token.IsAuthenticated);
        Assert.Equal($"Incorrect Credentials for user {tokenRequestModel.Email}.", token.Message);
    }

    [Fact]
    public async Task GetTokenAsync_ExistingUser_ProperResult()
    {
        var tokenRequestModel = new GetTokenResponseDto
        {
            Email = Authorization.DefaultEmail,
            Password = Authorization.DefaultPassword
        };

        var context = new ApplicationDbContextFakeSqliteBuilder().Build();
        var userStore = new UserStore<ApplicationUser>(context);
        var mockUserManager =
            Substitute.For<UserManager<ApplicationUser>>(userStore, null, null, null, null, null, null, null, null);
        mockUserManager.FindByEmailAsync(Arg.Any<string>())
            .Returns(new ApplicationUser
            {
                Id = $"{Authorization.DefaultUsername}Id",
                Email = Authorization.DefaultEmail,
                UserName = $"{Authorization.DefaultUsername}"
            });
        mockUserManager.CheckPasswordAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>())
            .Returns(true);
        mockUserManager.GetClaimsAsync(Arg.Any<ApplicationUser>())
            .Returns(new List<Claim>());
        mockUserManager.GetRolesAsync(Arg.Any<ApplicationUser>())
            .Returns(new List<string> {"User"});
        var mockLogger = Substitute.For<ILogger<UserService>>();
        var mockApplicationRoleLogger = Substitute.For<ILogger<ApplicationRoleService>>();
        var applicationRoleService = new ApplicationRoleService(context, mockApplicationRoleLogger);
        _sut = new UserService(mockUserManager, _jwt, mockLogger, context, applicationRoleService);

        var authenticationModel = await _sut.GetTokenAsync(tokenRequestModel);
        Assert.True(authenticationModel.IsAuthenticated);

        // トークン登録確認
        var tokens = context.RefreshTokens
            .Where(m => m.ApplicationUserId == "userId")
            .ToList();
        Assert.Single(tokens);
    }

    [Fact]
    public async Task CreateJwtToken_Test_NotNull()
    {
        var context = new ApplicationDbContextFakeSqliteBuilder().Build();
        var userStore = new UserStore<ApplicationUser>(context);
        var userManager = new UserManager<ApplicationUser>(userStore, null, null, null, null, null, null, null, null);
        var mockLogger = Substitute.For<ILogger<UserService>>();
        var mockApplicationRoleLogger = Substitute.For<ILogger<ApplicationRoleService>>();
        var applicationRoleService = new ApplicationRoleService(context, mockApplicationRoleLogger);
        _sut = new UserService(userManager, _jwt, mockLogger, context, applicationRoleService);

        var user = await _sut.GetByIdAsync($"{Authorization.DefaultUsername}Id");
        Assert.NotNull(user);
        var jwtToken = await _sut.CreateJwtToken(user);
        var result = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        Assert.NotNull(result);
    }
}
