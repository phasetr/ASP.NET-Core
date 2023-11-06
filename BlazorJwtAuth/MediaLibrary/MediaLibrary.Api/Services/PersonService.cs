using AutoMapper;
using MediaLibrary.Api.Data;
using MediaLibrary.Common.Entities;
using MediaLibrary.Common.Models;

namespace MediaLibrary.Api.Services;

public class PersonService : BaseService<Person, PersonModel>
{
    public PersonService(MediaLibraryDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}
