using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Birahe.EndPoint.Attribute;

public class PasswordAttribute : ValidationAttribute{
    public PasswordAttribute() {
        ErrorMessage = "نام کاربری معتبر نیست !";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        // [Required]
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()) || string.IsNullOrEmpty(value.ToString())) {
            return new ValidationResult("نام کاربری الزامی است.");
        }

        var password = value.ToString();
        // MinLength
        if (password.Length < 8) {
            return new ValidationResult("نام کاربری باید حداقل 8 کاراکتر باشد.");
        }
        //MaxLength
        if (password.Length > 70) {
            return new ValidationResult("نام کاربری میتواند حداکثر 70 کاراکتر باشد.");
        }
        // Only containing numbers, letters, and _
        if (!Regex.IsMatch(password, @"^[a-zA-Z0-9_@!-]+$"))
        {
            return new ValidationResult("نام کاربری فقط می‌تواند شامل حروف، اعداد یا _ ,@ ,! ,- ,باشد.");
        }

        return ValidationResult.Success!;
    }
}