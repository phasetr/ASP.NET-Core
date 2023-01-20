using System.ComponentModel.DataAnnotations;
using CityBreaks.Models;
using CityBreaks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CityBreaks.Pages;

public class CityModel : PageModel
{
    private readonly ICityService _cityService;
    private readonly ILogger<CityModel> _logger;
    private readonly IPropertyService _propertyService;

    public CityModel(ICityService cityService,
        IPropertyService propertyService,
        ILogger<CityModel> logger)
    {
        _cityService = cityService;
        _propertyService = propertyService;
        _logger = logger;
    }

    [BindProperty(SupportsGet = true)] public string Name { get; set; }

    public City City { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        City = await _cityService.GetByNameAsync(Name);
        if (City == null)
        {
            _logger.LogWarning("City \"{Name}\" not found", Name);
            return NotFound();
        }

        _logger.LogInformation("City \"{Name}\"  found", Name);
        return Page();
    }

    public async Task<PartialViewResult> OnGetPropertyDetails(int id)
    {
        var property = await _propertyService.FindAsync(id);
        if (User.Identity != null)
            _logger.LogInformation("Property {@Property} retrieved by {User}", property, User.Identity.Name);
        var model = new BookingInputModel {Property = property};
        return Partial("_PropertyDetailsPartial", model);
    }

    public JsonResult OnPostBooking([FromBody] BookingInputModel model)
    {
        if (model.EndDate == null || model.StartDate == null) return new JsonResult(new {TotalCost = 0.0});
        var numberOfDays = (int) (model.EndDate.Value - model.StartDate.Value).TotalDays;
        var totalCost = numberOfDays * model.Property.DayRate * model.NumberOfGuests;
        var result = new {TotalCost = totalCost};
        return new JsonResult(result);
    }

    public class BookingInputModel
    {
        public Property Property { get; set; }

        [Display(Name = "No. of guests")] public int NumberOfGuests { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Arrival")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Departure")]
        public DateTime? EndDate { get; set; }
    }
}