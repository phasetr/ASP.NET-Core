using MediaLibrary.Api.Services;
using MediaLibrary.Common.Entities;
using MediaLibrary.Common.Models;

namespace MediaLibrary.Api.Controllers;

public class MovieController(MovieService service)
    : BaseController<MovieModel, Movie, MovieService>(service, "/movies");
