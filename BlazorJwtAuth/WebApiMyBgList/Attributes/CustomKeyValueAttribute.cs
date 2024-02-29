namespace WebApiMyBgList.Attributes;

[AttributeUsage(
    AttributeTargets.Property | AttributeTargets.Parameter,
    AllowMultiple = true)]
public class CustomKeyValueAttribute(string? key, string? value) : Attribute
{
    public string? Key { get; set; } = key;

    public string? Value { get; set; } = value;
}
