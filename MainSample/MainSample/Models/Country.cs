namespace MainSample.Models;

public class Country
{
    public int Id { get; set; }
    public string CountryName { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public ICollection<City> Cities { get; set; }
}