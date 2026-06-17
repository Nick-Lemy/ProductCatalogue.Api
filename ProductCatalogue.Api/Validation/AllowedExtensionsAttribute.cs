using System.ComponentModel.DataAnnotations;

namespace ProductCatalogue.Api.Validation;

public class AllowedExtensionsAttribute(string[] extensions) : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not IFormFile file)
            return true;

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return extensions.Contains(extension);
    }

    public override string FormatErrorMessage(string name) =>
        $"Only the following file types are allowed: {string.Join(", ", extensions)}";
}