using System.Reflection;

namespace Client.Shared.Model;

public class TableColumn
{
    public string Name { get; set; } = string.Empty;
    public PropertyInfo PropertyInfo { get; set; } = null!;
}
