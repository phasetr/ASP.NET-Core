using MediaLibrary.Common;
using MediaLibrary.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediaLibrary.Api.Data.Configurations;

public class MovieCategoryConfiguration : IEntityTypeConfiguration<MovieCategory>
{
    public void Configure(EntityTypeBuilder<MovieCategory> builder)
    {
        builder.HasKey(x => new {x.Category, x.MovieId});
        builder.HasData(new MovieCategory
        {
            MovieId = 1,
            Category = CategoryType.Action
        });
    }
}
