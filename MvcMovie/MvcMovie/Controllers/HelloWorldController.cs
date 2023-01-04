using System.Text.Encodings.Web;
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
    // GET https://localhost:5500/helloworld/welcome/3?name=Rick
    public string Welcome(string name, int id=1)
    {
        return HtmlEncoder.Default.Encode($"Hello {name}, ID: {id}");
    }
}