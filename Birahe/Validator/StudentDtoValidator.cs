using Birahe.EndPoint.Models;
using Birahe.EndPoint.Models.Dto;
using FluentValidation;

namespace Birahe.EndPoint.Validator;

public class StudentDtoValidator : AbstractValidator<StudentDto> {
    public StudentDtoValidator() {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("نام  الزامی است.")
            .MinimumLength(3).WithMessage("نام باید حداقل 3 کاراکتر باشد.")
            .MaximumLength(70).WithMessage("نام میتواند حداکثر 70 کاراکتر باشد.")
            .Matches(@"^[\u0600-\u06FF\s]+$").WithMessage("نام فقط می‌تواند شامل حروف فارسی باشد.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("نام خانوادگی الزامی است.")
            .MinimumLength(3).WithMessage("نام خانوادگی باید حداقل 3 کاراکتر باشد.")
            .MaximumLength(70).WithMessage(" نام خانوادگی میتواند حداکثر 70 کاراکتر باشد.")
            .Matches(@"^[\u0600-\u06FF\s]+$").WithMessage("نام خانوادگی فقط می‌تواند شامل حروف فارسی باشد.");

        RuleFor(x => x.StudentNo)
            .NotEmpty().WithMessage("کد دانشجویی الزامی است.")
            .MinimumLength(10).WithMessage("کد دانشجویی باید حداقل 10 کاراکتر باشد.")
            .MaximumLength(11).WithMessage("کد دانشجویی میتواند حداکثر 11 کاراکتر باشد.")
            .Matches(@"^[0-9]+$").WithMessage("کد دانشجویی فقط می‌تواند شامل اعداد باشد.");

        RuleFor(x => x.Field)
            .NotEmpty().WithMessage("رشته تحصیلی الزامی است.")
            .MinimumLength(3).WithMessage("رشته تحصیلی باید حداقل 3 کاراکتر باشد.")
            .MaximumLength(120).WithMessage("رشته تحصیلی میتواند حداکثر 120 کاراکتر باشد.")
            .Matches(@"^[\u0600-\u06FF\s]+$").WithMessage("رشته تحصیلی فقط می‌تواند شامل حروف فارسی باشد.");

        RuleFor(x => x.IsMale)
            .NotNull()
            .Must(g => g == true || g == false).WithMessage("جنسیت باید مقدار بولین داشته باشد!");


    }
}