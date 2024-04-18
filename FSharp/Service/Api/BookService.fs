namespace Service.Api

open Common.Entities
open Common.Interfaces
open DataContext.Data
open Common.Helpers.MyAsync
open Microsoft.Extensions.Logging

type BookService(logger: ILogger<BookService>, context: ApplicationDbContext) =
  interface IBookService with
    member _.GetBookAsync(bookId: BookId) =
      try
        async {
          let! book = context.Books.FindAsync(bookId) |> awaitValueTask
          return book
        }
      with ex ->
        logger.LogError(ex, "Error in GetBookAsync")
        raise ex

    member _.AddBookAsync(book: Book) =
      try
        async {
          context.Books.AddAsync(book) |> ignore
          let! cnt = context.SaveChangesAsync() |> Async.AwaitTask
          return cnt
        }
      with ex ->
        logger.LogError(ex, "Error in AddBookAsync")
        raise ex
