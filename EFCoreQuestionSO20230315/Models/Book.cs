namespace EFCoreQuestionSO20230315.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public ICollection<BookCategory> BookCategories { get; set; }
}
