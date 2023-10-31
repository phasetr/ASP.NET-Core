using Api.Data;
using AutoMapper;
using Common.Entities;
using Common.Models;

namespace Api.Services;

public class PersonService : BaseService<Person, PersonModel>
{
    public PersonService(MediaLibraryDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}
