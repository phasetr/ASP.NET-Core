using Common.Constants;
using Common.EntityModels.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Service.Services;
using Test.Unit.FakesSqlite;

namespace Test.Unit.WebApi.Service.Service;

public class ApplicationRoleServiceTests
{
    [Fact]
    public async Task AddRoleToUserAsync_Succeeded()
    {
        var context = new ApplicationDbContextFakeSqliteBuilder().Build();
        var mockLogger = Substitute.For<ILogger<ApplicationRoleService>>();
        var sut = new ApplicationRoleService(context, mockLogger);

        var user = new ApplicationUser
        {
            Id = "userId",
            UserName = "user",
            Email = "user@secureapi.com",
            FirstName = "first",
            LastName = "last"
        };
        var roleName = Authorization.Roles.Administrator.ToString();
        var result = await sut.AddRoleToUserAsync(user, roleName);

        Assert.NotNull(result);
        Assert.Equal("Role added.", result.Message);
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task AddRoleToUserAsync_RoleNotFound()
    {
        var context = new ApplicationDbContextFakeSqliteBuilder().Build();
        var mockLogger = Substitute.For<ILogger<ApplicationRoleService>>();
        var sut = new ApplicationRoleService(context, mockLogger);

        var user = new ApplicationUser
        {
            Id = "userId",
            UserName = "user",
            Email = "user@secureapi.com",
            FirstName = "first",
            LastName = "last"
        };
        const string roleName = "No Role";
        var result = await sut.AddRoleToUserAsync(user, roleName);

        Assert.NotNull(result);
        Assert.Equal("Role not found.", result.Message);
        Assert.False(result.Succeeded);
    }
}
