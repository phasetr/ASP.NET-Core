using AutoMapper;
using Common.Entities;
using Common.Models;

namespace Api.Helper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // Person mappings
        CreateMap<Person, PersonModel>()
            .ForMember(x => x.MoviesIds, map => map.MapFrom(actor => actor.Movies.Select(x => x.MovieId)));

        CreateMap<PersonModel, Person>()
            .ForMember(x => x.Id, map => map.Ignore())
            .ForMember(x => x.Movies, map => map.MapFrom((model, entity) =>
            {
                var current = entity.Movies.Select(m => m.MovieId).ToArray();
                var movies = entity.Movies.ToList();
                movies.AddRange(model.MoviesIds.Where(x => !current.Contains(x)).Select(x => new MovieActor
                {
                    MovieId = x
                }));

                movies.RemoveAll(x => !model.MoviesIds.Contains(x.MovieId));

                return movies;
            }));

        // Movie mappings
        CreateMap<Movie, MovieModel>()
            .ForMember(x => x.Categories, map => map.MapFrom(movie => movie.Categories.Select(x => x.Category)))
            .ForMember(x => x.ActorIds, map => map.MapFrom(movie => movie.Actors.Select(x => x.PersonId)));

        CreateMap<MovieModel, Movie>()
            .ForMember(x => x.Id, map => map.Ignore())
            .ForMember(x => x.Categories, map => map.MapFrom((model, entity) =>
            {
                var current = entity.Categories.Select(x => x.Category).ToArray();
                var categories = entity.Categories.ToList();
                categories.AddRange(model.Categories.Where(x => !current.Contains(x)).Select(x => new MovieCategory
                {
                    Category = x
                }));
                categories.RemoveAll(x => !model.Categories.Contains(x.Category));
                return categories;
            }))
            .ForMember(x => x.Actors, map => map.MapFrom((model, entity) =>
            {
                var current = entity.Actors.Select(x => x.PersonId).ToArray();
                var actors = entity.Actors.ToList();
                actors.AddRange(model.ActorIds.Where(x => !current.Contains(x)).Select(x => new MovieActor
                {
                    PersonId = x
                }));

                actors.RemoveAll(x => !model.ActorIds.Contains(x.PersonId));

                return actors;
            }));
    }
}
