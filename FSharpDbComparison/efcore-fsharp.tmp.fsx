#r "nuget: Microsoft.EntityFrameworkCore.Sqlite"
#r "nuget: Microsoft.EntityFrameworkCore.Design"
#r "nuget: System.Linq"
#r "nuget: EntityFrameworkCore.FSharp"

open System.Collections.Generic
open Microsoft.EntityFrameworkCore
open EntityFrameworkCore.FSharp.Extensions
open EntityFrameworkCore.FSharp.DbContextHelpers

let dbname = "efcore-fsharp.tmp.tmp.db"

[<CLIMutable>]
type Book = {
    Id: int
    Title: string
    BookAuthors: BookAuthor list
}
and [<CLIMutable>]
Author = {
    Id: int
    Name: string
    BookAuthors: BookAuthor list
}
and [<CLIMutable>]
BookAuthor = {
    BookId: int
    AuthorId: int
    Book: Book
    Author: Author
}

type AppDbContext() =
    inherit DbContext()

    [<DefaultValue>]
    val mutable books : DbSet<Book>
    member this.Books with get() = this.books and set v = this.books <- v

    [<DefaultValue>]
    val mutable authors : DbSet<Author>
    member this.Authors with get() = this.authors and set v = this.authors <- v

    [<DefaultValue>]
    val mutable bookAuthors : DbSet<BookAuthor>
    member this.BookAuthors with get() = this.bookAuthors and set v = this.bookAuthors <- v

    override this.OnModelCreating(modelBuilder: ModelBuilder) =
        modelBuilder.Entity<Book>(fun b ->
            b.HasKey(fun book -> book.Id :> obj) |> ignore
            b.HasMany(fun book -> book.BookAuthors :> IEnumerable<BookAuthor>)
             .WithOne(fun ba -> ba.Book)
             .HasForeignKey(fun ba -> ba.BookId :> obj)
            |> ignore
        ) |> ignore

        modelBuilder.Entity<Author>(fun a ->
            a.HasKey(fun author -> author.Id :> obj) |> ignore
            a.HasMany(fun author -> author.BookAuthors :> IEnumerable<BookAuthor>)
             .WithOne(fun ba -> ba.Author)
             .HasForeignKey(fun ba -> ba.AuthorId :> obj)
            |> ignore
        ) |> ignore

        modelBuilder.Entity<BookAuthor>(fun ba ->
            ba.HasKey(fun bookAuthor -> (bookAuthor.BookId, bookAuthor.AuthorId) :> obj) |> ignore
        ) |> ignore

    override _.OnConfiguring(options: DbContextOptionsBuilder) : unit =
        options.UseSqlite($"Data Source={dbname}").UseFSharpTypes()
        |> ignore

let initializeDatabase() =
    if System.IO.File.Exists(dbname) then printfn "Database already exists"
    else
        use context = new AppDbContext()
        context.Database.EnsureCreated() |> ignore
        let users = [
            {Id = 1; Name = "Alice"; BookAuthors = []}
            {Id = 2; Name = "Bob"; BookAuthors = []}
            {Id = 3;Name = "Charlie"; BookAuthors = []}
        ]
        addEntityRange context users
        saveChanges context
        printfn "Database created and seeded"

let displayData() =
    use context = new AppDbContext()
    query {
        for author in context.Authors do
            select author
    }
    |> toListAsync
    |> Async.RunSynchronously
    |> Seq.iter (fun u -> printfn $"ID: %d{u.Id}, Name: %s{u.Name}")

// Run the initialization and display the data
initializeDatabase()
displayData()
