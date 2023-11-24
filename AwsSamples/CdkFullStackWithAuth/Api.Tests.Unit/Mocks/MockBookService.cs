using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Services.Interfaces;
using Bogus;
using Common.Entities;

namespace Api.Tests.Unit.Mocks;

internal class MockBookService : IBookService
{
    private readonly Faker<BookEntity> _fakeEntity;

    public MockBookService()
    {
        _fakeEntity = new Faker<BookEntity>()
            .RuleFor(o => o.Authors, f => new List<string> {f.Name.FullName(), f.Name.FullName()})
            .RuleFor(o => o.CoverPage, f => f.Image.LoremPixelUrl())
            .RuleFor(o => o.Id, _ => Guid.NewGuid());
    }

    public Task<bool> CreateAsync(BookEntity bookEntity)
    {
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(BookEntity bookEntity)
    {
        return Task.FromResult(true);
    }

    public Task<IList<BookEntity>> GetBooksAsync(int limit = 10)
    {
        IList<BookEntity> books = _fakeEntity.Generate(limit).ToList();

        return Task.FromResult(books);
    }

    public Task<BookEntity?> GetByIdAsync(Guid id)
    {
        _ = _fakeEntity.RuleFor(o => o.Id, _ => id);
        var book = _fakeEntity.Generate() ?? null;

        return Task.FromResult(book);
    }

    public Task<bool> UpdateAsync(BookEntity bookEntity)
    {
        return Task.FromResult(true);
    }
}
