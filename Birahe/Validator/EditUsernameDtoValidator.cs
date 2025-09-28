using Birahe.EndPoint.Models;
using Birahe.EndPoint.Models.Dto;
using FluentValidation;

namespace Birahe.EndPoint.Validator;

public class EditUsernameDtoValidator : AbstractValidator<EditUsernameDto> {
    public EditUsernameDtoValidator() {
        RuleFor(x => x.OldUsername)
            .NotEmpty().WithMessage("نام کاربری الزامی است.")
            .MinimumLength(4).WithMessage("نام کاربری باید حداقل ۴ کاراکتر باشد.")
            .MaximumLength(50).WithMessage("نام کاربری میتواند حداکثر 50 کاراکتر باشد.")
            .Matches(@"^[a-zA-Z0-9_]+$")
            .WithMessage("نام کاربری فقط می‌تواند شامل حروف، اعداد یا _ باشد.");

        RuleFor(x => x.NewUsername)
            .NotEmpty().WithMessage("نام کاربری الزامی است.")
            .MinimumLength(4).WithMessage("نام کاربری باید حداقل ۴ کاراکتر باشد.")
            .MaximumLength(50).WithMessage("نام کاربری میتواند حداکثر 50 کاراکتر باشد.")
            .Matches(@"^[a-zA-Z0-9_]+$")
            .WithMessage("نام کاربری فقط می‌تواند شامل حروف، اعداد یا _ باشد.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("رمز عبور الزامی است.")
            .MinimumLength(8).WithMessage("رمز عبور باید حداقل 8 کاراکتر باشد.")
            .MaximumLength(70).WithMessage("رمز عبور میتواند حداکثر 70 کاراکتر باشد.")
            .Matches(@"^[a-zA-Z0-9_@!-]+$").WithMessage("رمز عبور فقط می‌تواند شامل حروف، اعداد یا _ ,@ ,! ,- ,باشد.");

        RuleFor(x => x).Must(x => x.NewUsername != x.OldUsername)
            .WithMessage("نام کاربری جدید و قدیمی یکسان هستند!");
    }
}