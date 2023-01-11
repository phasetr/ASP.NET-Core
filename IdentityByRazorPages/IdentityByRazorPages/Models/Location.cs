namespace IdentityByRazorPages.Models;

public class Location
{
    public long LocationId { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;

    public IEnumerable<Person>? People { get; set; }
}