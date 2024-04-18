namespace Common.Entities

type BookId = int

type Book() =
  member val BookId: BookId = 0 with get, set
  member val Title: string = "" with get, set
