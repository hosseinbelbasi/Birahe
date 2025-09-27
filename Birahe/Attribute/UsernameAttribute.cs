using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Birahe.EndPoint.Attribute;

public class UsernameAttribute: ValidationAttribute {
    public UsernameAttribute() {
        ErrorMessage = "نام کاربری معتبر نیست!";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        // [Required]
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()) || string.IsNullOrEmpty(value.ToString())) {
            return new ValidationResult("نام کاربری الزامی است.");
        }

        var username = value.ToString();
        // MinLength
        if (username.Length < 4) {
            return new ValidationResult("نام کاربری باید حداقل ۴ کاراکتر باشد.");
        }
        //MaxLength
        if (username.Length > 50) {
            return new ValidationResult("نام کاربری میتواند حداکثر 50 کاراکتر باشد.");
        }
        // Only containing numbers, letters, and _
        if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
        {
            return new ValidationResult("نام کاربری فقط می‌تواند شامل حروف، اعداد یا _ باشد.");
        }

        return ValidationResult.Success!;
    }
}