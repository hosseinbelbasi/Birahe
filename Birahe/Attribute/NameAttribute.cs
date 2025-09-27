using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Birahe.EndPoint.Attribute;

public class NameAttribute : ValidationAttribute {
    public NameAttribute() {
        ErrorMessage = "نام یا نام خانوادگی معتبر نیست!";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        // [Required]
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()) || string.IsNullOrEmpty(value.ToString())) {
            return new ValidationResult("نام و نام خانوادگی الزامی است.");
        }

        var name = value.ToString();
        // MinLength
        if (name.Length < 3) {
            return new ValidationResult("نام یا نام خانوادگی باید حداقل 3 کاراکتر باشد.");
        }
        //MaxLength
        if (name.Length > 70) {
            return new ValidationResult("نام یا نام خانوادگی میتواند حداکثر 70 کاراکتر باشد.");
        }
        // Only containing numbers, letters, and _
        if (!Regex.IsMatch(name, @"^[\u0600-\u06FF\s]+$"))
        {
            return new ValidationResult("نام یا نام خانوادگی فقط می‌تواند شامل حروف فارسی باشد.");
        }

        return ValidationResult.Success!;
    }

}