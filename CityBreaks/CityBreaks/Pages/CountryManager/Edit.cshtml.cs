using CityBreaks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CityBreaks.Pages.CountryManager;

public class EditModel : PageModel
{
    [BindProperty] public List<InputModel> Inputs { get; set; }

    public List<Country> Countries { get; set; } = new();

    public void OnGet()
    {
        Inputs = new List<InputModel>
        {
            new() {Id = 840, CountryCode = "us", CountryName = "United States"},
            new() {Id = 826, CountryCode = "en", CountryName = "Great Britain"},
            new() {Id = 250, CountryCode = "fr", CountryName = "France"}
        };
    }

    public void OnPost()
    {
        Countries = Inputs
            .Where(x => !string.IsNullOrWhiteSpace(x.CountryCode))
            .Select(x => new Country
            {
                Id = x.Id,
                CountryCode = x.CountryCode,
                CountryName = x.CountryName
            }).ToList();
    }

    public class InputModel
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
    }
}