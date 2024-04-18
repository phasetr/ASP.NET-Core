namespace Api.Controllers

open Common.Interfaces
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

[<ApiController>]
[<Route("api/[controller]")>]
type BookController(logger: ILogger<BookController>, bookService: IBookService) =
  inherit ControllerBase()

  [<HttpGet>]
  member _.Get(bookId) =
    logger.LogInformation($"Getting book: {bookId}")

    async {
      let! book = bookService.GetBookAsync(bookId)
      return book
    }

  [<HttpPost>]
  member _.Post(book: Common.Entities.Book) =
    logger.LogInformation($"Adding book: {book.BookId}, {book.Title}")

    async {
      let! book = bookService.AddBookAsync(book)
      return book
    }
