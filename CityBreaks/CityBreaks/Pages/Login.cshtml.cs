using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CityBreaks.Pages;

public class LoginModel : PageModel
{
    [BindProperty]
    [Display(Name = "User name")]
    public string UserName { get; set; }

    public async Task OnPostAsync()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, UserName)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(principal);
    }
}