using BlazorJwtAuth.Common.EntityModels.Entities;
using BlazorJwtAuth.Common.Models;
using BlazorJwtAuth.Common.Services.Interfaces;
using BlazorJwtAuth.Common.Settings;
using BlazorJwtAuth.Test.Unit.FakesSqlite;
using BlazorJwtAuth.WebApi.Controllers.V1;
using BlazorJwtAuth.WebApi.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace BlazorJwtAuth.Test.Unit.WebApi.Controllers.V1;

public class UserControllerTests
{
    /// ユーザーが存在しないため認証失敗
    [Fact]
    public async Task GetTokenAsync_NonExistingUser_NotAuthenticated()
    {
        var context = new ApplicationDbContextFakeSqliteBuilder().Build();
        var userStore = new UserStore<ApplicationUser>(context);
        // パスワード認証があるため、ハッシャーも設定する
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        var userManager = new UserManager<ApplicationUser>(
            userStore, null, passwordHasher,
            null, null, null,
            null, null, null);
        var claimsService = new ClaimsService(userManager);
        var configuration = Substitute.For<IConfiguration>();
        configuration["Jwt:Key"].Returns("abcdefghijklmnopqrstuvwxyz012345");
        configuration["Jwt:DurationInMinutes"].Returns("30");
        configuration["Jwt:Issuer"].Returns("Issuer");
        configuration["Jwt:Audience"].Returns("Audience");
        var ptDateTime = Substitute.For<IPtDateTime>();
        var jwt = Substitute.For<IOptions<Jwt>>();
        jwt.Value.Returns(new Jwt
        {
            Key = "abcdefghijklmnopqrstuvwxyz012345",
            DurationInMinutes = 30,
            Issuer = "Issuer",
            Audience = "Audience"
        });
        var userService = new UserService(userManager, jwt, Substitute.For<ILogger<UserService>>(), context);
        var jwtTokenService = new JwtTokenService(configuration, ptDateTime);

        var controller = new UserController(claimsService, jwtTokenService, userService, userManager);
        var result = await controller.GetTokenAsync(new GetTokenRequest
        {
            Email = "nouser@secureapi.com",
            Password = "Pa$$w0rd."
        });
        var resultObject = Assert.IsType<OkObjectResult>(result);
        var okValue = Assert.IsAssignableFrom<AuthenticationResponse>(resultObject.Value);
        Assert.False(okValue.IsAuthenticated);
        Assert.Equal($"No Accounts Registered with nouser@secureapi.com.", okValue.Message);
    }

    /// パスワードが間違っているため認証失敗
    [Fact]
    public async Task GetTokenAsync_ExistingUserWithWrongPassword_NotAuthenticated()
    {
        var context = new ApplicationDbContextFakeSqliteBuilder().Build();
        var userStore = new UserStore<ApplicationUser>(context);
        // パスワード認証があるため、ハッシャーも設定する
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        var mockLoggerForUserManager = Substitute.For<ILogger<UserManager<ApplicationUser>>>();
        mockLoggerForUserManager.LogWarning("");
        var userManager = new UserManager<ApplicationUser>(
            userStore, null, passwordHasher,
            null, null, null,
            null, null, mockLoggerForUserManager);
        var claimsService = new ClaimsService(userManager);
        var configuration = Substitute.For<IConfiguration>();
        configuration["Jwt:Key"].Returns("abcdefghijklmnopqrstuvwxyz012345");
        configuration["Jwt:DurationInMinutes"].Returns("30");
        configuration["Jwt:Issuer"].Returns("Issuer");
        configuration["Jwt:Audience"].Returns("Audience");
        var ptDateTime = Substitute.For<IPtDateTime>();
        var jwt = Substitute.For<IOptions<Jwt>>();
        jwt.Value.Returns(new Jwt
        {
            Key = "abcdefghijklmnopqrstuvwxyz012345",
            DurationInMinutes = 30,
            Issuer = "Issuer",
            Audience = "Audience"
        });
        var mockLogger = Substitute.For<ILogger<UserService>>();
        mockLogger.LogInformation("");
        var userService = new UserService(userManager, jwt, mockLogger, context);
        var jwtTokenService = new JwtTokenService(configuration, ptDateTime);

        var controller = new UserController(claimsService, jwtTokenService, userService, userManager);
        var result = await controller.GetTokenAsync(new GetTokenRequest
        {
            Email = "user@secureapi.com",
            Password = "WrongPassword"
        });
        Assert.NotNull(result);
        var resultObject = Assert.IsType<OkObjectResult>(result);
        var okValue = Assert.IsAssignableFrom<AuthenticationResponse>(resultObject.Value);
        Assert.False(okValue.IsAuthenticated);
        Assert.Equal($"Incorrect Credentials for user user@secureapi.com.", okValue.Message);
    }

    /// ユーザーが存在してパスワードが正しい場合
    [Fact]
    public async Task GetTokenAsync_ExistingUserWithProperPassword_Ok()
    {
        var context = new ApplicationDbContextFakeSqliteBuilder().Build();
        var userStore = new UserStore<ApplicationUser>(context);
        // パスワード認証があるため、ハッシャーも設定する
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        var userManager = new UserManager<ApplicationUser>(
            userStore, null, passwordHasher,
            null, null, null,
            null, null, null);
        var claimsService = new ClaimsService(userManager);
        var configuration = Substitute.For<IConfiguration>();
        configuration["Jwt:Key"].Returns("abcdefghijklmnopqrstuvwxyz012345");
        configuration["Jwt:DurationInMinutes"].Returns("30");
        configuration["Jwt:Issuer"].Returns("Issuer");
        configuration["Jwt:Audience"].Returns("Audience");
        var ptDateTime = Substitute.For<IPtDateTime>();
        var jwt = Substitute.For<IOptions<Jwt>>();
        jwt.Value.Returns(new Jwt
        {
            Key = "abcdefghijklmnopqrstuvwxyz012345",
            DurationInMinutes = 30,
            Issuer = "Issuer",
            Audience = "Audience"
        });
        var userService = new UserService(userManager, jwt, Substitute.For<ILogger<UserService>>(), context);
        var jwtTokenService = new JwtTokenService(configuration, ptDateTime);

        var controller = new UserController(claimsService, jwtTokenService, userService, userManager);
        var result = await controller.GetTokenAsync(new GetTokenRequest
        {
            Email = "user@secureapi.com",
            Password = "Pa$$w0rd."
        });
        Assert.NotNull(result);
        var resultObject = Assert.IsType<OkObjectResult>(result);
        var okValue = Assert.IsAssignableFrom<AuthenticationResponse>(resultObject.Value);
        Assert.True(okValue.IsAuthenticated);
        Assert.Equal("Token Created Properly.", okValue.Message);
    }
}
