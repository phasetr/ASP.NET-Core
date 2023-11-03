﻿using System.ComponentModel.DataAnnotations;

namespace WebApiMyBgList.Attributes;

public class SortColumnValidatorAttribute : ValidationAttribute
{
    public SortColumnValidatorAttribute(Type entityType)
        : base("Value must match an existing column.")
    {
        EntityType = entityType;
    }

    public Type EntityType { get; set; }

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
