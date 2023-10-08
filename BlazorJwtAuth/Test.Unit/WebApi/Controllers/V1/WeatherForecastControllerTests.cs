using WebApi.Controllers.V1;

namespace Test.Unit.WebApi.Controllers.V1;

public class WeatherForecastControllerTests
{
    [Fact]
    public void Get_Should_Return_Ok()
    {
        var random = new Random();
        var controller = new WeatherForecastController(random);
        var result = controller.Get();
        Assert.NotNull(result);
        Assert.Equal(5, result.Count());
    }
}
