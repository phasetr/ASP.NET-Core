using Api.Services.Interfaces;
using Common.Constants;
using Common.Dto;
using Common.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route(ApiPath.BookRoot)]
[Produces("application/json")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet("{limit:int}")]
    public async Task<ActionResult<BookGetDto>> Get(int limit = 10)
    {
        if (limit is <= 0 or > 100)
            return BadRequest(new BookGetDto
            {
                Message = "The limit should been between [1-100]",
                Succeeded = false
            });

        var books = await _bookService.GetBooksAsync(limit);
        return Ok(new BookGetDto
        {
            Message = "Succeeded",
            Succeeded = true,
            Books = books
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookEntity>> Get(Guid id)
    {
        var result = await _bookService.GetByIdAsync(id);

        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<BookEntity>> Post([FromBody] BookPostDto? dto)
    {
        if (dto == null) return ValidationProblem("Invalid input! Book not informed");
        var book = new BookEntity
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] BookEntity? book)
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
