using Api.Data;
using AutoMapper;
using Common.Entities;
using Common.Models;

namespace Api.Services;

public class MovieService : BaseService<Movie, MovieModel>
{
    public MovieService(MediaLibraryDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}
