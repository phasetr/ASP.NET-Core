#r "nuget: Microsoft.EntityFrameworkCore.Sqlite"
#r "nuget: Microsoft.EntityFrameworkCore.Design"
#r "nuget: System.Linq"

open System.Linq
open Microsoft.EntityFrameworkCore

// Define the entity
type Person() =
    member val Id = 0 with get, set
    member val Name = "" with get, set
    member val Age = 0 with get, set

// Define the DbContext
type AppDbContext() =
    inherit DbContext()

    [<DefaultValue>]
    val mutable people: DbSet<Person>
    member this.People with get() = this.people and set v = this.people <- v

    override this.OnConfiguring(optionsBuilder: DbContextOptionsBuilder) =
        optionsBuilder.UseSqlite("Data Source=1.tmp.db") |> ignore

// Initialize the database and add some data
let initializeDatabase() =
    use context = new AppDbContext()
    context.Database.EnsureCreated() |> ignore

    if not (context.People.Any()) then
        let people = [
            Person(Id = 1, Name = "Alice", Age = 30)
            Person(Id = 2, Name = "Bob", Age = 25)
            Person(Id = 3, Name = "Charlie", Age = 35)
        ]
        context.People.AddRange(people)
        context.SaveChanges() |> ignore

let displayData() =
    use context = new AppDbContext()
    let people = context.People.ToList()
    for person in people do
        printfn "ID: %d, Name: %s, Age: %d" person.Id person.Name person.Age

// Run the initialization and display the data
initializeDatabase()
displayData()
