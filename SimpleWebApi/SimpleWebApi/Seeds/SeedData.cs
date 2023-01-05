using Microsoft.EntityFrameworkCore;
using SimpleWebApi.Models;

namespace SimpleWebApi.Seeds;

public static class SeedData
{
    public static void EnsurePopulated(IApplicationBuilder app)
    {
        var context = app.ApplicationServices
            .CreateScope().ServiceProvider.GetRequiredService<TodoContext>();

        if (context.Database.GetPendingMigrations().Any()) context.Database.Migrate();

        if (!context.TodoItems.Any())
        {
            context.TodoItems.AddRange(
                new TodoItem {Name = "TODO1", IsComplete = true, Secret = "secret1"},
                new TodoItem {Name = "TODO2", IsComplete = false, Secret = "secret2"}
            );
            context.SaveChanges();
        }
    }
}