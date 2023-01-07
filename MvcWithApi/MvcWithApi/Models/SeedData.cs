using Microsoft.EntityFrameworkCore;
using MvcWithApi.Data;

namespace MvcWithApi.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new MyDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<MyDbContext>>());
        // Look for any movies.
        if (context.Movie.Any() && context.TodoItems.Any()) return; // DB has been seeded

        context.Movie.AddRange(
            new Movie
            {
                Title = "When Harry Met Sally",
                ReleaseDate = DateTime.Parse("1989-2-12"),
                Genre = "Romantic Comedy",
                Rating = "R",
                Price = 7.99M
            },
            new Movie
            {
                Title = "GhostBusters ",
                ReleaseDate = DateTime.Parse("1984-3-13"),
                Genre = "Comedy",
                Rating = "R",
                Price = 8.99M
            },
            new Movie
            {
                Title = "GhostBusters 2",
                ReleaseDate = DateTime.Parse("1986-2-23"),
                Genre = "Comedy",
                Rating = "R",
                Price = 9.99M
            },
            new Movie
            {
                Title = "Rio Bravo",
                ReleaseDate = DateTime.Parse("1959-4-15"),
                Genre = "Western",
                Rating = "R",
                Price = 3.99M
            }
        );

        context.TodoItems.AddRange(
            new TodoItem {Name = "Read a book", IsComplete = false, Secret = "secret1"},
            new TodoItem {Name = "Write code", IsComplete = false, Secret = "secret2"},
            new TodoItem {Name = "Cook", IsComplete = true, Secret = "secret3"}
        );

        context.SaveChanges();
    }
}