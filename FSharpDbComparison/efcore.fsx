#r "nuget: Microsoft.EntityFrameworkCore.Sqlite"
#r "nuget: Microsoft.EntityFrameworkCore.Design"
#r "nuget: System.Linq"

open System.Linq
open Microsoft.EntityFrameworkCore

// Define the entity
type User() =
    member val Id = 0 with get, set
    member val Name = "" with get, set
    member val Age = 0 with get, set

// Define the DbContext
type AppDbContext() =
    inherit DbContext()

    [<DefaultValue>]
    val mutable users: DbSet<User>
    member this.Users with get() = this.users and set v = this.users <- v

    override this.OnConfiguring(optionsBuilder: DbContextOptionsBuilder) =
        optionsBuilder.UseSqlite("Data Source=efcore.db") |> ignore

// Initialize the database and add some data
let initializeDatabase() =
    use context = new AppDbContext()
    context.Database.EnsureCreated() |> ignore

    if not (context.Users.Any()) then
        let users = [
            User(Id = 1, Name = "Alice", Age = 30)
            User(Id = 2, Name = "Bob", Age = 25)
            User(Id = 3, Name = "Charlie", Age = 35)
        ]
        context.Users.AddRange(users)
        context.SaveChanges() |> ignore

let displayData() =
    use context = new AppDbContext()
    let users = context.Users.ToList()
    for user in users do
        printfn $"ID: %d{user.Id}, Name: %s{user.Name}, Age: %d{user.Age}"

// Run the initialization and display the data
initializeDatabase()
displayData()
