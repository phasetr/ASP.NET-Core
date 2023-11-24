using Common.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route(ApiPath.CalculatorRoot)]
public class CalculatorController : ControllerBase
{
    private readonly ILogger<CalculatorController> _logger;

    public CalculatorController(ILogger<CalculatorController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     Perform x + y
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>Sum of x and y.</returns>
    [HttpGet("add/{x:int}/{y:int}")]
    public int Add(int x, int y)
    {
        _logger.LogInformation("{X} plus {Y} is {XY}", x, y, x + y);
        return x + y;
    }

    /// <summary>
    ///     Perform x - y.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>x subtract y</returns>
    [HttpGet("subtract/{x:int}/{y:int}")]
    public int Subtract(int x, int y)
    {
        _logger.LogInformation("{X} subtract {Y} is {XY}", x, y, x - y);
        return x - y;
    }

    /// <summary>
    ///     Perform x * y.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>x multiply y</returns>
    [HttpGet("multiply/{x:int}/{y:int}")]
    public int Multiply(int x, int y)
    {
        _logger.LogInformation("{X} multiply {Y} is {XY}", x, y, x * y);
        return x * y;
    }

    /// <summary>
    ///     Perform x / y.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>x divide y</returns>
    [HttpGet("divide/{x:int}/{y:int}")]
    public int Divide(int x, int y)
    {
        _logger.LogInformation("{X} divide {Y} is {XY}", x, y, x / y);
        return x / y;
    }
}
