using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Birahe.EndPoint.Attribute;

public class FieldAttribute : ValidationAttribute {
    public FieldAttribute() {
        ErrorMessage = "رشته تحصیلی معتبر نیست!";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        // [Required]
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()) || string.IsNullOrEmpty(value.ToString())) {
            return new ValidationResult("رشته تحصیلی الزامی است.");
        }

        var name = value.ToString();
        // MinLength
        if (name.Length < 3) {
            return new ValidationResult("رشته تحصیلی باید حداقل 3 کاراکتر باشد.");
        }
        //MaxLength
        if (name.Length > 120) {
            return new ValidationResult("رشته تحصیلی میتواند حداکثر 120 کاراکتر باشد.");
        }
        // Only containing numbers, letters, and _
        if (!Regex.IsMatch(name, @"^[\u0600-\u06FF\s]+$"))
        {
            return new ValidationResult("رشته تحصیلی فقط می‌تواند شامل حروف فارسی باشد.");
        }

        return ValidationResult.Success!;
    }

}