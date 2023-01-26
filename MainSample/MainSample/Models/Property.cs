namespace MainSample.Models;

public class Property
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int CityId { get; set; }
    public City City { get; set; }
    public int MaxNumberOfGuests { get; set; }
    public decimal DayRate { get; set; }
    public bool SmokingPermitted { get; set; }
    public DateTime AvailableFrom { get; set; }
    public DateTime? Deleted { get; set; }
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; }
}