namespace MediaLibrary.Client.Shared.Model;

public class TableRow<TItem>(TItem originValue)
{
    public List<TableCell> Values { get; set; } = new();
    public TItem OriginValue { get; set; } = originValue;
}
