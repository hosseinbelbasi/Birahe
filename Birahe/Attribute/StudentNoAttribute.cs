using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Birahe.EndPoint.Attribute;

public class StudentNoAttribute : ValidationAttribute{
    public StudentNoAttribute() {
        ErrorMessage = "کد دانشجویی معتبر نیست!";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {

        // [Required]
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()) || string.IsNullOrEmpty(value.ToString())) {
            return new ValidationResult("کد دانشجویی الزامی است.");
        }

        var password = value.ToString();
        // MinLength
        if (password.Length < 10) {
            return new ValidationResult("کد دانشجویی باید حداقل 10 کاراکتر باشد.");
        }
        //MaxLength
        if (password.Length > 11) {
            return new ValidationResult("کد دانشجویی میتواند حداکثر 11 کاراکتر باشد.");
        }
        // Only containing numbers, letters, and _
        if (!Regex.IsMatch(password, @"^[0-9]+$"))
        {
            return new ValidationResult("کد دانشجویی فقط می‌تواند شامل اعداد باشد.");
        }

        return ValidationResult.Success!;

    }
}