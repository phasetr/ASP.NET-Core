using Common;
using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.Property(x => x.Name).IsRequired();
        builder.HasOne(x => x.Director).WithMany().HasForeignKey(x => x.DirectorId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.MusicComposer).WithMany().HasForeignKey(x => x.MusicComposerId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Navigation(x => x.Director).AutoInclude();
        builder.Navigation(x => x.MusicComposer).AutoInclude();
        builder.Navigation(x => x.Actors).AutoInclude();

        builder.HasData(new
        {
            Id = 1,
            Name = "Movie1",
            Categories = new
            {
                MovieCategory = new MovieCategory
                {
                    MovieId = 1,
                    Category = CategoryType.Action
                }
            },
            Year = 2021,
            Description = "A movie about a movie",
            DirectorId = 1,
            MusicComposerId = 1
        });
    }
}
