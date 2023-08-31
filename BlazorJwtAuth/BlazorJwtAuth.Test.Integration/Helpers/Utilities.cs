using BlazorJwtAuth.Common.DataContext.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorJwtAuth.Test.Integration.Helpers;

public static class Utilities
{
    public static void InitializeDbForTests(ApplicationDbContext db)
    {
        db.Database.OpenConnection();
        db.Database.EnsureCreated();
        db.SaveChanges();
    }

    public static void ReinitializeDbForTests(ApplicationDbContext db)
    {
        db.Database.EnsureDeleted();
        InitializeDbForTests(db);
    }
}
