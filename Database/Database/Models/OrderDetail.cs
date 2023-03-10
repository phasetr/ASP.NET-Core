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
    ///     OrderId
    /// </summary>
    public long OrderId { get; set; }

    public Order Order { get; set; } = new();

    /// <summary>
    ///     ProductId
    /// </summary>
    public long ProductId { get; set; }

    public Product Product { get; set; } = new();
}