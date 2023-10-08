using Common.EntityModels.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Test.Unit.FakesSqlite;
using WebApi.Service.Services;

namespace Test.Unit.WebApi.Service.Service;

public partial class UserServiceTests
{
    [Fact]
    public async Task GetById_ExistingUser()
    {
        var context = new ApplicationDbContextFakeSqliteBuilder().Build();
        var userStore = new UserStore<ApplicationUser>(context);
        var userManager = new UserManager<ApplicationUser>(userStore, null, null, null, null, null, null, null, null);
        var mockLogger = Substitute.For<ILogger<UserService>>();
        var mockApplicationRoleLogger = Substitute.For<ILogger<ApplicationRoleService>>();
        var applicationRoleService = new ApplicationRoleService(context, mockApplicationRoleLogger);
        _sut = new UserService(userManager, _jwt, mockLogger, context, applicationRoleService);

        var result = await _sut.GetByIdAsync("userId");

        Assert.NotNull(result);
        Assert.Equal("userId", result.Id);
    }
}
