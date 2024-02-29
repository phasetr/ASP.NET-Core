using AutoMapper;
using MediaLibrary.Common.Entities;
using MediaLibrary.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaLibrary.Api.Services;

public class PersonService(DbContext dbContext, IMapperBase mapper)
    : BaseService<Person, PersonModel>(dbContext, mapper);
