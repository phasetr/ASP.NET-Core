namespace WebApiMyBgList.Dto;

public class LinkDto(string href, string rel, string type)
{
    public string Href { get; private set; } = href;
    public string Rel { get; private set; } = rel;
    public string Type { get; private set; } = type;
}
