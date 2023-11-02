using Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Service.Services;
using Test.Unit.FakesSqlite;

namespace Test.Unit.Service;

public class ClaimsServiceTests : SqliteMemoryBase
{
    private ClaimsService _sut = default!;

    [Fact]
    public async Task GetUserClaimsAsync_CreateClaims()
    {
        var context = new ApplicationDbContextFakeSqliteBuilder().Build();
        var userStore = new UserStore<ApplicationUser>(context);
        var userManager = new UserManager<ApplicationUser>(userStore, null, null, null, null, null, null, null, null);
        _sut = new ClaimsService(userManager);

        var user = new ApplicationUser
        {
            Email = "noUser@phasetr.com",
            UserName = "noUser"
        };
        var claims = await _sut.GetUserClaimsAsync(user);

        Assert.Equal(2, claims.Count);
        Assert.Equal("noUser", claims[0].Value);
        Assert.Equal("noUser@phasetr.com", claims[1].Value);
    }

    [Fact]
    public async Task GetUserClaimsAsync_ExistingUserWithRoles_CreateClaims()
    {
        var context = new ApplicationDbContextFakeSqliteBuilder().Build();
        var userStore = new UserStore<ApplicationUser>(context);
        var userManager = new UserManager<ApplicationUser>(userStore, null, null, null, null, null, null, null, null);
        _sut = new ClaimsService(userManager);

        var user = new ApplicationUser
        {
            Id = "userId",
            Email = "user@secureapi.com",
            UserName = "user"
        };
        var claims = await _sut.GetUserClaimsAsync(user);

        Assert.Equal(3, claims.Count);
        Assert.Equal("user", claims[0].Value);
        Assert.Equal("user@secureapi.com", claims[1].Value);
        Assert.Equal("User", claims[2].Value);
    }
}
