using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages.Samples;

public class Index : PageModel
{
    public string Message { get; set; } = string.Empty;
    public string PostMessage { get; set; } = string.Empty;

    [BindProperty] public string CityName { get; set; } = string.Empty;

    public void OnGet(int id)
    {
        Message = $"OnGet executed with id = {id}";
    }

    public void OnPostSearch(string searchTerm)
    {
        PostMessage = $"You searched for {searchTerm}";
    }

    public void OnPostRegister(string email)
    {
        PostMessage = $"You registered {email} for newsletters";
    }
}