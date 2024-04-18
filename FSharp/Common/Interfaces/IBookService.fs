namespace Common.Interfaces

open Common.Entities

type IBookService =
  abstract member GetBookAsync: BookId -> Async<Book>
  abstract member AddBookAsync: Book -> Async<int>
