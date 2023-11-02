using Common.Constants;
using Common.Dto;
using Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Service.Services;
using Test.Unit.FakesSqlite;

namespace Test.Unit.Service;

public partial class UserServiceTests
{
    [Fact]
    public async Task AddRoleAsync_Success()
    {
        var user = new ApplicationUser
        {
            Id = "userId",
            UserName = Authorization.DefaultUsername,
            NormalizedUserName = Authorization.DefaultUsername.ToUpper(),
            Email = Authorization.DefaultEmail,
            NormalizedEmail = Authorization.DefaultEmail.ToUpper(),
            EmailConfirmed = true,
            FirstName = "First",
            LastName = "Last"
        };
        var context = new ApplicationDbContextFakeSqliteBuilder().Build();
        var userStore = new UserStore<ApplicationUser>(context);
        var mockUserManager =
            Substitute.For<UserManager<ApplicationUser>>(userStore, null, null, null, null, null, null, null, null);
        mockUserManager.FindByEmailAsync(user.Email).Returns(user);
        var mockLogger = Substitute.For<ILogger<UserService>>();
        var mockApplicationRoleLogger = Substitute.For<ILogger<ApplicationRoleService>>();
        var applicationRoleService = new ApplicationRoleService(context, mockApplicationRoleLogger);
        _sut = new UserService(mockUserManager, _jwt, mockLogger, context, applicationRoleService);
        var dto = new AddRoleDto
        {
            Email = Authorization.DefaultEmail,
            Role = Authorization.Roles.Administrator.ToString()
        };

        var result = await _sut.AddRoleAsync(dto);

        Assert.Equal($"Added {Authorization.Roles.Administrator} to user {dto.Email}.", result);
    }

    [Fact]
    public async Task AddRoleAsync_Failure_NoUser()
    {
        var context = new ApplicationDbContextFakeSqliteBuilder().Build();
        var userStore = new UserStore<ApplicationUser>(context);
        var userManager = new UserManager<ApplicationUser>(userStore, null, null, null, null, null, null, null, null);
        var mockLogger = Substitute.For<ILogger<UserService>>();
        var mockApplicationRoleLogger = Substitute.For<ILogger<ApplicationRoleService>>();
        var applicationRoleService = new ApplicationRoleService(context, mockApplicationRoleLogger);
        _sut = new UserService(userManager, _jwt, mockLogger, context, applicationRoleService);
        var dto = new AddRoleDto
        {
            Email = Authorization.DefaultEmail,
            Role = Authorization.Roles.Administrator.ToString()
        };

        var result = await _sut.AddRoleAsync(dto);

        Assert.Equal($"No Accounts Registered with {dto.Email}.", result);
    }

    [Fact]
    public async Task AddRoleAsync_Failure_NoRole()
    {
        var user = new ApplicationUser
        {
            Id = "userId",
            UserName = Authorization.DefaultUsername,
            NormalizedUserName = Authorization.DefaultUsername.ToUpper(),
            Email = Authorization.DefaultEmail,
            NormalizedEmail = Authorization.DefaultEmail.ToUpper(),
            EmailConfirmed = true,
            FirstName = "First",
            LastName = "Last"
        };
        var context = new ApplicationDbContextFakeSqliteBuilder().Build();
        var userStore = new UserStore<ApplicationUser>(context);
        var mockUserManager =
            Substitute.For<UserManager<ApplicationUser>>(userStore, null, null, null, null, null, null, null, null);
        mockUserManager.FindByEmailAsync(user.Email).Returns(user);
        var mockLogger = Substitute.For<ILogger<UserService>>();
        var mockApplicationRoleLogger = Substitute.For<ILogger<ApplicationRoleService>>();
        var applicationRoleService = new ApplicationRoleService(context, mockApplicationRoleLogger);
        _sut = new UserService(mockUserManager, _jwt, mockLogger, context, applicationRoleService);
        var dto = new AddRoleDto
        {
            Email = Authorization.DefaultEmail,
            Role = "NoRole"
        };

        var result = await _sut.AddRoleAsync(dto);

        Assert.Equal("Role NoRole not found.", result);
    }
}
