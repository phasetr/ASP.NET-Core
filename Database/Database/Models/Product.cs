namespace Database.Models;

/// <summary>
///     Products
/// </summary>
public class Product
{
    /// <summary>
    ///     ProductId
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    ///     Name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    ///     Description
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    ///     Price
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    ///     Category
    /// </summary>
    public string Category { get; set; } = null!;
}