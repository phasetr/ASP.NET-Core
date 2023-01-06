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
    public Product Product { get; set; } = new();

    /// <summary>
    ///     Quantity
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    ///     OrderID
    /// </summary>
    public Order Order { get; set; } = new();
}