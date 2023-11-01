using Common.Dto;
using Entities.DynamoDb;
using Microsoft.AspNetCore.Mvc;
using ServerlessApi.Service.Interfaces;

namespace ServerlessApi.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    // GET api/books
    [HttpGet]
    public async Task<ActionResult<BookGetResponseDto>> Get([FromQuery] int limit = 10)
    {
        if (limit is <= 0 or > 100)
            return BadRequest(new BookGetResponseDto
            {
                Message = "The limit should been between [1-100]",
                IsSucceed = false
            });

        var books = await _bookService.GetBooksAsync(limit);
        return Ok(new BookGetResponseDto
        {
            Message = "Succeeded",
            IsSucceed = true,
            Books = books
        });
    }

    // GET api/books/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> Get(Guid id)
    {
        var result = await _bookService.GetByIdAsync(id);

        if (result == null) return NotFound();

        return Ok(result);
    }

    // POST api/books
    [HttpPost]
    public async Task<ActionResult<Book>> Post([FromBody] BookPostDto? dto)
    {
        if (dto == null) return ValidationProblem("Invalid input! Book not informed");
        var book = new Book
        {
            Id = Guid.Empty,
            Title = dto.Title,
            Isbn = dto.Isbn ?? "",
            Authors = dto.Authors ?? new List<string>()
        };

        var result = await _bookService.CreateAsync(book);

        if (result)
            return CreatedAtAction(
                nameof(Get),
                new {id = book.Id},
                book);
        return BadRequest("Fail to persist");
    }

    // PUT api/books/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] Book? book)
    {
        if (id == Guid.Empty || book is null) return ValidationProblem("Invalid request payload");

        // Retrieve the book.
        var bookRetrieved = await _bookService.GetByIdAsync(id);

        if (bookRetrieved == null)
        {
            var errorMsg = $"Invalid input! No book found with id:{id}";
            return NotFound(errorMsg);
        }

        book.Id = bookRetrieved.Id;

        await _bookService.UpdateAsync(book);
        return Ok();
    }

    // DELETE api/books/5
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty) return ValidationProblem("Invalid request payload");

        var bookRetrieved = await _bookService.GetByIdAsync(id);

        if (bookRetrieved == null)
        {
            var errorMsg = $"Invalid input! No book found with id:{id}";
            return NotFound(errorMsg);
        }

        await _bookService.DeleteAsync(bookRetrieved);
        return Ok();
    }
}
