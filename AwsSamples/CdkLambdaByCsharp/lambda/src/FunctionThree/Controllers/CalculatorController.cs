using Microsoft.AspNetCore.Mvc;

namespace FunctionThree.Controllers;

[ApiController]
[Route("[controller]")]
public class CalculatorController(ILogger<CalculatorController> logger) : ControllerBase
{
    [HttpGet("add/{x:int}/{y:int}")]
    public string Add(int x, int y)
    {
        logger.LogInformation("{X} plus {Y} is {XY}", x, y, x + y);
        return $"{x} + {y} = {x + y}\n";
    }

    [HttpGet("subtract/{x:int}/{y:int}")]
    public string Subtract(int x, int y)
    {
        logger.LogInformation("{X} subtract {Y} is {XY}", x, y, x - y);
        return $"{x} - {y} = {x - y}\n";
    }

    [HttpGet("multiply/{x:int}/{y:int}")]
    public string Multiply(int x, int y)
    {
        logger.LogInformation("{X} multiply {Y} is {X}", x, y, x * y);
        return $"{x} * {y} = {x * y}\n";
    }

    [HttpGet("divide/{x:int}/{y:int}")]
    public string Divide(int x, int y)
    {
        if (y == 0)
        {
            logger.LogError("Cannot divide by zero");
            return "Cannot divide by zero\n";
        }

        logger.LogInformation("{X} divide {Y} is {XY}", x, y, x / y);
        return $"{x} / {y} = {x / y}\n";
    }
}
