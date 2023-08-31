using BlazorJwtAuth.Common.EntityModels.Entities;
using BlazorJwtAuth.Test.Unit.FakesSqlite;
using BlazorJwtAuth.WebApi.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace BlazorJwtAuth.Test.Unit.WebApi.Service.Service;

public partial class UserServiceTests
{
    [Fact]
    public async Task GetById_ExistingUser()
    {
        var context = new ApplicationDbContextFakeSqliteBuilder().Build();
        var userStore = new UserStore<ApplicationUser>(context);
        var userManager = new UserManager<ApplicationUser>(userStore, null, null, null, null, null, null, null, null);
        var mockLogger = Substitute.For<ILogger<UserService>>();
        _sut = new UserService(userManager, _jwt, mockLogger, context);

        var result = await _sut.GetByIdAsync("userId");

        Assert.NotNull(result);
        Assert.Equal("userId", result.Id);
    }
}
