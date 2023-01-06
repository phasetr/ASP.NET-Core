namespace Database.Models;

/// <summary>
///     OrderDetails
/// </summary>
public class OrderDetail
{
    /// <summary>
    ///     Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     OrderID
    /// </summary>
    public Order Order { get; set; } = new();

    /// <summary>
    ///     ProductID
    /// </summary>
    public Product Product { get; set; } = new();
}