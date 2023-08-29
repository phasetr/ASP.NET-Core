using BlazorJwtAuth.Common.DataContext.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorJwtAuth.Test.Unit.FakesSqlite;

public class SqliteMemoryBase : IDisposable
{
    protected readonly ApplicationDbContext Context;

    protected SqliteMemoryBase()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;
        Context = new ApplicationDbContext(options);
        Context.Database.OpenConnection();
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }

    public ApplicationDbContext Build()
    {
        Context.SaveChanges();
        return Context;
    }
}
