using MediaLibrary.Api.Services;
using MediaLibrary.Common.Entities;
using MediaLibrary.Common.Models;

namespace MediaLibrary.Api.Controllers;

public class MovieController : BaseController<MovieModel, Movie, MovieService>
{
    public MovieController(MovieService service) : base(service, "/movies")
    {
    }
}
