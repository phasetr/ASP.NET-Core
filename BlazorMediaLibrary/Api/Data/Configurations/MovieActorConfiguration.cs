using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Configurations;

public class MovieActorConfiguration : IEntityTypeConfiguration<MovieActor>
{
    public void Configure(EntityTypeBuilder<MovieActor> builder)
    {
        builder.HasKey(x => new {x.MovieId, x.PersonId});
        builder.HasData(new MovieActor
        {
            MovieId = 1,
            PersonId = 1
        });
    }
}
