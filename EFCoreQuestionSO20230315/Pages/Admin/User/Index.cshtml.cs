using EFCoreQuestionSO20230315.Models;
using EFCoreQuestionSO20230315.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EFCoreQuestionSO20230315.Pages.Admin.User;

public class IndexModel : PageModel
{
    private readonly IApplicationUserService _applicationUserService;

    public IndexModel(IApplicationUserService applicationUserService)
    {
        _applicationUserService = applicationUserService;
    }

    public IList<ApplicationUser> ApplicationUsers { get; set; } = default!;

    public async Task OnGetAsync()
    {
        var applicationUsers = await _applicationUserService.GetAllAsync();
        ApplicationUsers = applicationUsers;
    }
}
