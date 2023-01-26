namespace MainSample.Models;

public class Pie
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int PieCategoryId { get; set; }
    public PieCategory PieCategory { get; set; }
}