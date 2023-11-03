﻿using System.ComponentModel.DataAnnotations;

namespace WebApiMyBgList.Attributes;

public class SortOrderValidatorAttribute : ValidationAttribute
{
    public SortOrderValidatorAttribute()
        : base("Value must be one of the following: {0}.")
    {
    }

    public string[] AllowedValues { get; } =
        {"ASC", "DESC"};

    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        var strValue = value as string;
        if (!string.IsNullOrEmpty(strValue)
            && AllowedValues.Contains(strValue))
            return ValidationResult.Success;

        return new ValidationResult(
            FormatErrorMessage(string.Join(",", AllowedValues))
        );
    }
}
