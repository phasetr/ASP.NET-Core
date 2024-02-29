using System.ComponentModel.DataAnnotations;

namespace WebApiMyBgList.Attributes;

public class SortColumnValidatorAttribute(Type entityType) : ValidationAttribute("Value must match an existing column.")
{
    public Type EntityType { get; set; } = entityType;

    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (EntityType != null)
        {
            var strValue = value as string;
            if (!string.IsNullOrEmpty(strValue)
                && EntityType.GetProperties()
                    .Any(p => p.Name == strValue))
                return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage);
    }
}
