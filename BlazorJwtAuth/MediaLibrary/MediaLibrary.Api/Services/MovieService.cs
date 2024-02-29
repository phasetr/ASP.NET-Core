using AutoMapper;
using MediaLibrary.Api.Data;
using MediaLibrary.Common.Entities;
using MediaLibrary.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaLibrary.Api.Services;

public class MovieService(DbContext dbContext, IMapper mapper)
    : BaseService<Movie, MovieModel>(dbContext, mapper);
