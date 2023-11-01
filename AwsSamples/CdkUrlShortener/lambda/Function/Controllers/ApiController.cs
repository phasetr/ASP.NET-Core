using Microsoft.AspNetCore.Mvc;

namespace Function.Controllers;

[ApiController]
[Route("api")]
public class ApiController : ControllerBase
{
    private readonly ILogger<ApiController> _logger;

    public ApiController(ILogger<ApiController> logger)
    {
        _logger = logger;
    }

    [HttpGet("add/{x:int}/{y:int}")]
    public int Add(int x, int y)
    {
        _logger.LogInformation("{X} plus {Y} is {XY}", x, y, x + y);
        return x + y;
    }

    [HttpGet("subtract/{x:int}/{y:int}")]
    public int Subtract(int x, int y)
    {
        _logger.LogInformation("{X} subtract {Y} is {XY}", x, y, x - y);
        return x - y;
    }

    [HttpGet("multiply/{x:int}/{y:int}")]
    public int Multiply(int x, int y)
    {
        _logger.LogInformation("{X} multiply {Y} is {X}", x, y, x * y);
        return x * y;
    }

    [HttpGet("divide/{x:int}/{y:int}")]
    public int Divide(int x, int y)
    {
        _logger.LogInformation("{X} divide {Y} is {XY}", x, y, x / y);
        return x / y;
    }
}
