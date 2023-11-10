using System.ComponentModel.DataAnnotations;

namespace CityBreaks.ValidationAttributes;

public class UploadFileExtensionsAttribute : ValidationAttribute
{
    private IEnumerable<string> _allowedExtensions;
    public string Extensions { get; set; }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        _allowedExtensions = Extensions?
            .Split(new[] {','},
                StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.ToLowerInvariant());
        if (value is IFormFile file && _allowedExtensions.Any())
        {
            var extension = Path.GetExtension(file.FileName.ToLowerInvariant());
            if (!_allowedExtensions.Contains(extension))
                return new ValidationResult(ErrorMessage
                                            ?? $"The file extension must be {Extensions}");
        }

        return ValidationResult.Success;
    }
}
