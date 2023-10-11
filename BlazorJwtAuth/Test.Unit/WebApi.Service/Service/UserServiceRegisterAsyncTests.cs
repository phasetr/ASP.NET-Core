using Common.DataContext.Data;
using Common.Dto;
using Common.EntityModels.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Service.Services;
using Test.Unit.FakesSqlite;

namespace Test.Unit.WebApi.Service.Service;

public partial class UserServiceTests
{
    [Fact]
    public async Task RegisterAsync_NewUser()
    {
        var context = new ApplicationDbContextFakeSqliteBuilder().Build();
        var userStore = new UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext>(context);
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        var userManager =
            new UserManager<ApplicationUser>(userStore, null, passwordHasher, null, null, null, null, null, null);
        var mockLogger = Substitute.For<ILogger<UserService>>();
        var mockApplicationRoleLogger = Substitute.For<ILogger<ApplicationRoleService>>();
        var applicationRoleService = new ApplicationRoleService(context, mockApplicationRoleLogger);
        _sut = new UserService(userManager, _jwt, mockLogger, context, applicationRoleService);

        var userRegisterDto = new UserRegisterDto
        {
            Email = "newUser@phasetr.com",
            FirstName = "New",
            LastName = "User",
            Password = "newUserPassword",
            Username = "newUser"
        };

        var user = new ApplicationUser
        {
            UserName = userRegisterDto.Username,
            Email = userRegisterDto.Email,
            FirstName = userRegisterDto.FirstName,
            LastName = userRegisterDto.LastName,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        var result = await _sut.RegisterAsync(userRegisterDto);

        Assert.NotNull(result);
        Assert.Equal($"User Registered with username {user.UserName}", result.Message);
    }
}
