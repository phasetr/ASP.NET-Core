using Common.DataContext.Data;
using Microsoft.EntityFrameworkCore;

namespace Test.Unit.FakesSqlite;

public class SqliteMemoryBase : IDisposable
{
    private readonly ApplicationDbContext _context;

    protected SqliteMemoryBase()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;
        _context = new ApplicationDbContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    public ApplicationDbContext Build()
    {
        _context.SaveChanges();
        return _context;
    }
}
