using Database.Context;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Database.Controllers;

public class ProductController : Controller
{
    private readonly MyDbContext _context;

    public ProductController(MyDbContext context)
    {
        _context = context;
    }

    // GET: Product
    public async Task<IActionResult> Index()
    {
        return View(await _context.Products.ToListAsync());
    }

    // GET: Product/Details/5
    public async Task<IActionResult> Details(long? id)
    {
        if (id == null) return NotFound();

        var product = await _context.Products
            .FirstOrDefaultAsync(m => m.ProductId == id);
        if (product == null) return NotFound();

        return View(product);
    }

    // GET: Product/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Product/Create
    // To protect from over-posting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProductId,Name,Description,Price,Category")] Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(product);
    }

    // GET: Product/Edit/5
    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null) return NotFound();

        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();
        return View(product);
    }

    // POST: Product/Edit/5
    // To protect from over-posting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, [Bind("ProductId,Name,Description,Price,Category")] Product product)
    {
        if (id != product.ProductId) return NotFound();

        if (!ModelState.IsValid) return View(product);
        try
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(product.ProductId))
                return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));

    }

    // GET: Product/Delete/5
    public async Task<IActionResult> Delete(long? id)
    {
        if (id == null) return NotFound();

        var product = await _context.Products
            .FirstOrDefaultAsync(m => m.ProductId == id);
        if (product == null) return NotFound();

        return View(product);
    }

    // POST: Product/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null) _context.Products.Remove(product);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(long id)
    {
        return (_context.Products.Any(e => e.ProductId == id));
    }
}