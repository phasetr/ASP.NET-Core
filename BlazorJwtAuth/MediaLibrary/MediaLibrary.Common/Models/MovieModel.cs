﻿namespace MediaLibrary.Common.Models;

public class MovieModel : IModel
{
    public List<CategoryType> Categories { get; set; } = [];
    public int Year { get; set; }
    public string? Description { get; set; }
    public int? DirectorId { get; set; }
    public int? MusicComposerId { get; set; }
    public int[] ActorIds { get; set; } = [];
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
