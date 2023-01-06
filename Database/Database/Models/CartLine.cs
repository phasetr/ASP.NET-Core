namespace Database.Models;

/// <summary>
///     CartLine
/// </summary>
public class CartLine
{
    /// <summary>
    ///     CartLineId
    /// </summary>
    public int CartLineId { get; set; }

    /// <summary>
    ///     ProductId
    /// </summary>
    public long ProductId { get; set; }

    public Product Product { get; set; } = new();

    /// <summary>
    ///     Quantity
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    ///     OrderId
    /// </summary>
    public long OrderId { get; set; }

    public Order Order { get; set; } = new();
}