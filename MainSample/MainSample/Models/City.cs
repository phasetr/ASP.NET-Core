﻿namespace MainSample.Models;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public Country Country { get; set; }
    public ICollection<Property> Properties { get; set; }
}