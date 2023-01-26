namespace MainSample.Models;

public class Pie
{
    public int PieId { get; set; }
    public string Name { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }
}