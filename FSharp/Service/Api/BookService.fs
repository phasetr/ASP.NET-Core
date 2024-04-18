namespace Service.Api

open Common.Entities
open Common.Interfaces
open DataContext.Data
open Common.Helpers.MyAsync

type BookService(context: ApplicationDbContext) =
  interface IBookService with
    member _.GetBookAsync(bookId: BookId) =
      async {
        let! book = context.Books.FindAsync(bookId) |> awaitValueTask
        return book
      }

    member _.AddBookAsync(book: Book) =
      async {
        context.Books.AddAsync(book) |> ignore
        let! cnt = context.SaveChangesAsync() |> Async.AwaitTask
        return cnt
      }

