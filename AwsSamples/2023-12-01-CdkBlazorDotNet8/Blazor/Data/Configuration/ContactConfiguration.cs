using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blazor.Data.Configuration;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    // Magic string.
    public static readonly string RowVersion = nameof(RowVersion);

    // A sampling of cities.
    private readonly string[] _cities =
    {
        "Austin",
        "Denver",
        "Fayetteville",
        "Des Moines",
        "San Francisco",
        "Portland",
        "Monroe",
        "Redmond",
        "Bothel",
        "Woodinville",
        "Kent",
        "Kennesaw",
        "Marietta",
        "Atlanta",
        "Lead",
        "Spokane",
        "Bellevue",
        "Seattle"
    };

    // Combined with things for last names.
    private readonly string[] _colors =
    {
        "Blue",
        "Aqua",
        "Red",
        "Green",
        "Orange",
        "Yellow",
        "Black",
        "Violet",
        "Brown",
        "Crimson",
        "Gray",
        "Cyan",
        "Magenta",
        "White",
        "Gold",
        "Pink",
        "Lavender"
    };

    // More uniqueness.
    private readonly string[] _directions =
    {
        "N",
        "NE",
        "E",
        "SE",
        "S",
        "SW",
        "W",
        "NW"
    };

    // Use these to make names.
    private readonly string[] _gems =
    {
        "Diamond",
        "Crystal",
        "Morion",
        "Azore",
        "Sapphire",
        "Cobalt",
        "Aquamarine",
        "Montana",
        "Turquoise",
        "Lime",
        "Erinite",
        "Emerald",
        "Turmaline",
        "Jonquil",
        "Olivine",
        "Topaz",
        "Citrine",
        "Sun",
        "Quartz",
        "Opal",
        "Alabaster",
        "Rose",
        "Burgundy",
        "Siam",
        "Ruby",
        "Amethyst",
        "Violet",
        "Lilac"
    };

    private readonly Random _random = new();

    // State list.
    private readonly string[] _states =
    {
        "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL",
        "GA", "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA",
        "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE",
        "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "OH", "OK",
        "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT",
        "VA", "WA", "WV", "WI", "WY"
    };

    // Street names.
    private readonly string[] _streets =
    {
        "Broad",
        "Wide",
        "Main",
        "Pine",
        "Ash",
        "Poplar",
        "First",
        "Third"
    };

    // Types of streets.
    private readonly string[] _streetTypes =
    {
        "Street",
        "Lane",
        "Place",
        "Terrace",
        "Drive",
        "Way"
    };

    // Also helpful for names.
    private readonly string[] _things =
    {
        "beard",
        "finger",
        "hand",
        "toe",
        "stalk",
        "hair",
        "vine",
        "street",
        "son",
        "brook",
        "river",
        "lake",
        "stone",
        "ship"
    };

    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        // The property, RowVersion, isn't on the C# class,
        // so we set it up as a "shadow" property and use it for concurrency.
        builder.Property<byte[]>(RowVersion).IsRowVersion();
        var list = Enumerable.Range(1, 500)
            .Select(MakeContact).ToList();
        builder.HasData(list);
    }

    // Picks a random item from a list.
    // list: A list of string to parse.
    private string RandomOne(IReadOnlyList<string> list)
    {
        var idx = _random.Next(list.Count - 1);
        return list[idx];
    }

    // Make a new contact.
    // Returns a random Contact instance.
    private Contact MakeContact(int id)
    {
        var contact = new Contact
        {
            Id = id,
            FirstName = RandomOne(_gems),
            LastName = $"{RandomOne(_colors)}{RandomOne(_things)}",
            Phone = $"({_random.Next(100, 999)})-555-{_random.Next(1000, 9999)}",
            Street = $"{_random.Next(1, 99999)} {_random.Next(1, 999)}" +
                     $" {RandomOne(_streets)} {RandomOne(_streetTypes)} {RandomOne(_directions)}",
            City = RandomOne(_cities),
            State = RandomOne(_states),
            ZipCode = $"{_random.Next(10000, 99999)}"
        };

        return contact;
    }
}
