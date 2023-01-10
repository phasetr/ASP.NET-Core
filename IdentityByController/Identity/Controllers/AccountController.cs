using Identity.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(
        ILogger<AccountController> logger,
        UserManager<IdentityUser> userMgr,
        SignInManager<IdentityUser> signInMgr)
    {
        _logger = logger;
        _userManager = userMgr;
        _signInManager = signInMgr;
    }

    public ViewResult Login(string returnUrl)
    {
        return View(new LoginModel
        {
            ReturnUrl = returnUrl
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (!ModelState.IsValid) return View(loginModel);
        var user =
            await _userManager.FindByNameAsync(loginModel.Name);
        if (user != null)
        {
            await _signInManager.SignOutAsync();
            if ((await _signInManager.PasswordSignInAsync(user,
                    loginModel.Password, false, false)).Succeeded)
                return Redirect(loginModel.ReturnUrl);
        }

        ModelState.AddModelError("", "Invalid name or password");

        return View(loginModel);
    }

    [Authorize]
    public async Task<RedirectResult> Logout(string returnUrl = "/")
    {
        await _signInManager.SignOutAsync();
        return Redirect(returnUrl);
    }
}