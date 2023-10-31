using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.Property(x => x.Name).IsRequired();
        builder.Navigation(x => x.Movies).AutoInclude();

        builder.HasData(new
        {
            Id = 1,
            Name = "Person1",
            BirthDay = new DateTime(1990, 1, 1),
            BirthPlace = "New York",
            Biography = "A person",
            Movies = new
            {
                MovieActor1 = new
                {
                    MovieId = 1,
                    PersonId = 1
                },
                MovieActor2 = new
                {
                    MovieId = 2,
                    PersonId = 1
                }
            }
        });
        builder.HasData(new
        {
            Id = 2,
            Name = "Person2",
            BirthDay = new DateTime(1991, 1, 1),
            BirthPlace = "Washington",
            Biography = "A person2"
        });
        builder.HasData(new
        {
            Id = 3,
            Name = "Person3",
            BirthDay = new DateTime(1992, 1, 1),
            BirthPlace = "Los Angeles",
            Biography = "A person3"
        });
    }
}
