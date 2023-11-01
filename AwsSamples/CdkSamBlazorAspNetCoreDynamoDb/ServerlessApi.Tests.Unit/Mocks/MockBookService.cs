using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Entities.DynamoDb;
using ServerlessApi.Service.Interfaces;

namespace ServerlessApi.Tests.Unit.Mocks;

internal class MockBookService : IBookService
{
    private readonly Faker<Book> _fakeEntity;

    public MockBookService()
    {
        _fakeEntity = new Faker<Book>()
            .RuleFor(o => o.Authors, f => new List<string> {f.Name.FullName(), f.Name.FullName()})
            .RuleFor(o => o.CoverPage, f => f.Image.LoremPixelUrl())
            .RuleFor(o => o.Id, _ => Guid.NewGuid());
    }

    public Task<bool> CreateAsync(Book book)
    {
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(Book book)
    {
        return Task.FromResult(true);
    }

    public Task<IList<Book>> GetBooksAsync(int limit = 10)
    {
        IList<Book> books = _fakeEntity.Generate(limit).ToList();

        return Task.FromResult(books);
    }

    public Task<Book?> GetByIdAsync(Guid id)
    {
        _ = _fakeEntity.RuleFor(o => o.Id, _ => id);
        var book = _fakeEntity.Generate() ?? null;

        return Task.FromResult(book);
    }

    public Task<bool> UpdateAsync(Book book)
    {
        return Task.FromResult(true);
    }
}
