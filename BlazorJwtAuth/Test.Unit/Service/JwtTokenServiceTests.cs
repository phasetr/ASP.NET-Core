using System.Security.Claims;
using Common.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Service.Services;
using Test.Unit.FakesSqlite;

namespace Test.Unit.Service;

public class JwtTokenServiceTests : SqliteMemoryBase
{
    [Fact]
    public void GetJwtToken_ReturnsJwtSecurityToken()
    {
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration["Jwt:Key"].Returns("C1CF4B7DC4C4175B6618DE4F55CA4as#");
        mockConfiguration["Jwt:DurationInMinutes"].Returns("1");
        mockConfiguration["Jwt:Issuer"].Returns("SecureApi");
        mockConfiguration["Jwt:Audience"].Returns("SecureApiUser");
        var mockPtDateTime = Substitute.For<IPtDateTime>();
        var dateTime = new DateTime(2021, 1, 1, 1, 1, 1, DateTimeKind.Utc);
        mockPtDateTime.UtcNow.Returns(dateTime);
        var sut = new JwtTokenService(mockConfiguration, mockPtDateTime);
        var userClaims = new List<Claim>
        {
            new(ClaimTypes.Name, "user"),
            new(ClaimTypes.Email, "user@secureapi.com")
        };

        var result = sut.GetJwtToken(userClaims);
        var claims = result.Claims.ToList();

        Assert.Equal("SecureApi", result.Issuer);
        Assert.Equal("SecureApiUser", result.Audiences.First());
        Assert.Equal("user", claims[0].Value);
        Assert.Equal("user@secureapi.com", claims[1].Value);
        Assert.Equal(dateTime.AddMinutes(1), result.ValidTo);
    }
}
