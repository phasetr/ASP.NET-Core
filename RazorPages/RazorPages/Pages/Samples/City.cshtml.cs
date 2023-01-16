using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages.Samples;

public class City : PageModel
{
    public string CityName { get; set; } = string.Empty;
    public int? Rating { get; set; }
    public void OnGet(string cityName, int? rating)
    {
        CityName = cityName;
        Rating = rating;
    }
}
