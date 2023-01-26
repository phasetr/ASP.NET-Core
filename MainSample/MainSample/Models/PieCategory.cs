namespace MainSample.Models;

public class PieCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Pie> Pies { get; set; }
}