namespace Database.Models;

/// <summary>
///     CartLine
/// </summary>
public class CartLine
{
    /// <summary>
    ///     CartLineID
    /// </summary>
    public int CartLineId { get; set; }

    /// <summary>
    ///     ProductID
    /// </summary>
    public long ProductId { get; set; }

    public Product Product { get; set; } = new();

    /// <summary>
    ///     Quantity
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    ///     OrderID
    /// </summary>
    public long OrderId { get; set; }

    public Order Order { get; set; } = new();
}