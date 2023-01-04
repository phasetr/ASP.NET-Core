namespace SportsStore.Models;

public class Cart
{
    public List<CartLine> Lines { get; set; } = new();

    public virtual void AddItem(Product product, int quantity)
    {
        var line = Lines
            .Where(p => p.Product.ProductID == product.ProductID)
            .FirstOrDefault();

        if (line == null)
            Lines.Add(new CartLine
            {
                Product = product,
                Quantity = quantity
            });
        else
            line.Quantity += quantity;
    }

    public virtual void RemoveLine(Product product)
    {
        Lines.RemoveAll(l => l.Product.ProductID == product.ProductID);
    }

    public decimal ComputeTotalValue()
    {
        return Lines.Sum(e => e.Product.Price * e.Quantity);
    }

    public virtual void Clear()
    {
        Lines.Clear();
    }
}

public class CartLine
{
    public int CartLineID { get; set; }
    public Product Product { get; set; } = new();
    public int Quantity { get; set; }
}