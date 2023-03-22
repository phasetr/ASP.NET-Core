using EFCoreQuestionSO20230315.Data;
using EFCoreQuestionSO20230315.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EFCoreQuestionSO20230315.Pages.Admin.Book;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Models.Book> Book { get; set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Book != null) Book = await _context.Book
            .Include(book => book.BookCategories)!
            .ThenInclude(bookCategory => bookCategory.Category)
            .ToListAsync();
    }
}
