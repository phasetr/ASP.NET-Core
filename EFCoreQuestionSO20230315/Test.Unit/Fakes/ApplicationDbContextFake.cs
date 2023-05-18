using EFCoreQuestionSO20230315.Data;
using Microsoft.EntityFrameworkCore;

namespace Test.Unit.Fakes;

public class ApplicationDbContextFake : ApplicationDbContext
{
    public ApplicationDbContextFake() : base(new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase($"Test-{Guid.NewGuid()}")
        .Options)
    {
    }
}
