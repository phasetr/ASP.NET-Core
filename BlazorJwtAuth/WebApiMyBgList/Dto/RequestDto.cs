using System.ComponentModel.DataAnnotations;
using WebApiMyBgList.Attributes;

namespace WebApiMyBgList.Dto;

public class RequestDto<T> : IValidatableObject
{
    [System.ComponentModel.DefaultValue(0)]
    public int PageIndex { get; set; } = 0;

    [System.ComponentModel.DefaultValue(10)]
    [Range(1, 100)]
    public int PageSize { get; set; } = 10;

    [System.ComponentModel.DefaultValue("Name")]
    public string? SortColumn { get; set; } = "Name";

    [SortOrderValidator]
    [System.ComponentModel.DefaultValue("ASC")]
    public string? SortOrder { get; set; } = "ASC";

    [System.ComponentModel.DefaultValue(null)]
    public string? FilterQuery { get; set; } = null;

    public IEnumerable<ValidationResult> Validate(
        ValidationContext validationContext)
    {
        var validator = new SortColumnValidatorAttribute(typeof(T));
        var result = validator
            .GetValidationResult(SortColumn, validationContext);
        return result != null
            ? new[] {result}
            // ReSharper disable once UseArrayEmptyMethod
            : new ValidationResult[0];
    }
}
