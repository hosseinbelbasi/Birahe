using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Models.Dto.UserDto_s;
using FluentValidation;

namespace Birahe.EndPoint.Validator.UserDtoValidators;

public class SignUpDtoValidator : AbstractValidator<SignUpDto> {
    public SignUpDtoValidator() {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("نام کاربری الزامی است.")
            .MinimumLength(4).WithMessage("نام کاربری باید حداقل ۴ کاراکتر باشد.")
            .MaximumLength(50).WithMessage("نام کاربری میتواند حداکثر 50 کاراکتر باشد.")
            .Matches(@"^[a-zA-Z0-9_]+$")
            .WithMessage("نام کاربری فقط می‌تواند شامل حروف، اعداد یا _ باشد.");

        RuleFor(x => x.TeamName)
            .NotEmpty().WithMessage("نام تیم الزامی است.")
            .MinimumLength(4).WithMessage("نام تیم باید حداقل ۴ کاراکتر باشد.")
            .MaximumLength(50).WithMessage("نام تیم میتواند حداکثر 50 کاراکتر باشد.")
            .Matches(@"^[a-zA-Z\u0600-\u06FF\s0-9_!]+$")
            .WithMessage("نام تیم فقط می‌تواند شامل حروف، اعداد یا _ ، ! باشد.");



        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("رمز عبور الزامی است.")
            .MinimumLength(8).WithMessage("رمز عبور باید حداقل 8 کاراکتر باشد.")
            .MaximumLength(70).WithMessage("رمز عبور میتواند حداکثر 70 کاراکتر باشد.")
            .Matches(@"^[a-zA-Z0-9_@!-]+$").
            WithMessage("رمز عبور فقط می‌تواند شامل حروف، اعداد یا _ ,@ ,! ,- ,باشد.");

        RuleFor(x => x.Students)
            .NotNull().WithMessage("دانشجویان الزامی است.")
            .Must(list => list.Count >= 2 && list.Count <= 3)
            .WithMessage("یک تیم باید حداقل 2 و حداکثر 3 دانشجو داشته باشد.");

        RuleForEach(x => x.Students).SetValidator(new StudentDtoValidator());

        RuleFor(x => x.Students)
            .Must(list => list.Select(s => s.StudentNo).Distinct().Count() == list.Count)
            .WithMessage("شماره دانشجویی تکراری در درخواست وجود دارد.");



    }
}