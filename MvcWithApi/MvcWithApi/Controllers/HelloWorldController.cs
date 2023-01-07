using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;

namespace MvcWithApi.Controllers;

public class HelloWorldController : Controller
{
    // GET: https://localhost:5500/HelloWorld/
    public IActionResult Index()
    {
        return View();
    }

    // GET: https://localhost:5500/helloWorld/welcome/?name=Rick&numtimes=4
    public IActionResult Welcome(string name, int numTimes = 1)
    {
        ViewData["Message"] = "Hello " + name;
        ViewData["NumTimes"] = numTimes;

        return View();
    }
}