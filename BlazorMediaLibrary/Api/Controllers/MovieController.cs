using Api.Services;
using Common.Entities;
using Common.Models;

namespace Api.Controllers;

public class MovieController : BaseController<MovieModel, Movie, MovieService>
{
    public MovieController(MovieService service) : base(service, "/movies")
    {
    }
}
