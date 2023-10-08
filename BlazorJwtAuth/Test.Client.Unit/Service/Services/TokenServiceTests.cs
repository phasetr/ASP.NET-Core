using System;
using System.Threading.Tasks;
using Client.Service.Services;
using Common.Dto;

namespace Test.Client.Unit.Service.Services;

public class TokenServiceTests : TestContext
{
    private TokenService _sut = default!;

    [Fact]
    public async Task GetTokenAsync_Test()
    {
        var localStorage = this.AddBlazoredLocalStorage();
        _sut = new TokenService(localStorage);
        var tokenDto = new TokenDto
        {
            Token = "token",
            Expiration = DateTime.UtcNow.AddDays(1)
        };
        await _sut.SetTokenAsync(tokenDto);

        var resultToken = await _sut.GetTokenAsync();
        Assert.Equal(tokenDto.Expiration, resultToken.Expiration);
        Assert.Equal(tokenDto.Token, resultToken.Token);
    }

    [Fact]
    public async Task RemoveTokenAsync_Test()
    {
        var localStorage = this.AddBlazoredLocalStorage();
        _sut = new TokenService(localStorage);
        var tokenDto = new TokenDto
        {
            Token = "token",
            Expiration = DateTime.UtcNow.AddDays(1)
        };
        await _sut.SetTokenAsync(tokenDto);

        var resultToken = await _sut.GetTokenAsync();
        Assert.Equal(tokenDto.Expiration, resultToken.Expiration);
        Assert.Equal(tokenDto.Token, resultToken.Token);

        await _sut.RemoveTokenAsync();
        var resultToken2 = await _sut.GetTokenAsync();
        Assert.Null(resultToken2);
    }
}
