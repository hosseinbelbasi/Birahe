using System.ComponentModel.DataAnnotations;

namespace Birahe.EndPoint.Attribute;

public class NotWhiteSpaceAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string s && string.IsNullOrWhiteSpace(s))
        {
            return new ValidationResult($"{validationContext.DisplayName} cannot be empty or whitespace.");
        }
        return ValidationResult.Success;
    }
}