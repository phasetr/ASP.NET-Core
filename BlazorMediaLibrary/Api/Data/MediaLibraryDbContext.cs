using Api.Data.Configurations;
using Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class MediaLibraryDbContext : DbContext
{
    public MediaLibraryDbContext(DbContextOptions<MediaLibraryDbContext> options) : base(options)
    {
    }

    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<MovieActor> MovieActors => Set<MovieActor>();
    public DbSet<MovieCategory> MovieCategories => Set<MovieCategory>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder
            .ApplyConfiguration(new PersonConfiguration())
            .ApplyConfiguration(new MovieConfiguration())
            .ApplyConfiguration(new MovieActorConfiguration())
            .ApplyConfiguration(new MovieCategoryConfiguration());
    }
}
