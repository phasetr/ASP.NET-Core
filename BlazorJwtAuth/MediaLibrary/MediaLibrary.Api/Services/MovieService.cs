using AutoMapper;
using MediaLibrary.Api.Data;
using MediaLibrary.Common.Entities;
using MediaLibrary.Common.Models;

namespace MediaLibrary.Api.Services;

public class MovieService : BaseService<Movie, MovieModel>
{
    public MovieService(MediaLibraryDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}
