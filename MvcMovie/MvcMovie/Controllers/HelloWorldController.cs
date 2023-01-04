using Microsoft.AspNetCore.Mvc;

namespace MvcMovie.Controllers;

public class HelloWorldController : Controller
{
    // GET https://localhost:5500/helloworld/
    public IActionResult Index()
    {
        return View();
    }

    // GET https://localhost:5500/helloworld/welcome
    // GET https://localhost:7156/HelloWorld/Welcome?name=Rick&numtimes=4
    public IActionResult Welcome(string name, int numTimes = 1)
    {
        ViewData["Message"] = "Hello " + name;
        ViewData["NumTimes"] = numTimes;
        return View();
    }
}