namespace Database.Models;

/// <summary>
///     Orders
/// </summary>
public class Order
{
    /// <summary>
    ///     OrderID
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    ///     Name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    ///     Line1
    /// </summary>
    public string Line1 { get; set; } = null!;

    /// <summary>
    ///     Line2
    /// </summary>
    public string? Line2 { get; set; }

    /// <summary>
    ///     Line3
    /// </summary>
    public string? Line3 { get; set; }

    /// <summary>
    ///     City
    /// </summary>
    public string City { get; set; } = null!;

    /// <summary>
    ///     State
    /// </summary>
    public string State { get; set; } = null!;

    /// <summary>
    ///     Zip
    /// </summary>
    public string? Zip { get; set; }

    /// <summary>
    ///     Country
    /// </summary>
    public string Country { get; set; } = null!;

    /// <summary>
    ///     GiftWrap
    /// </summary>
    public bool GiftWrap { get; set; }

    /// <summary>
    ///     Shipped
    /// </summary>
    public bool Shipped { get; set; }
}