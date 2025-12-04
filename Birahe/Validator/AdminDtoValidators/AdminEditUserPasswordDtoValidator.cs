using Birahe.EndPoint.Models.Dto.AdminDto_s;
using FluentValidation;

namespace Birahe.EndPoint.Validator;

public class AdminEditUserPasswordDtoValidator : AbstractValidator<AdminEditUserPasswordDto> {
    public AdminEditUserPasswordDtoValidator() {
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("رمز عبور جدید الزامی است.")
            .MinimumLength(8).WithMessage("رمز عبور جدید باید حداقل 8 کاراکتر باشد.")
            .MaximumLength(70).WithMessage("رمز عبور جدید میتواند حداکثر 70 کاراکتر باشد.")
            .Matches(@"^[a-zA-Z0-9_@!-]+$")
            .WithMessage("رمز عبور جدید فقط می‌تواند شامل حروف، اعداد یا _ ,@ ,! ,- ,باشد.");

        RuleFor(x => x.NewPassswordRepeat)
            .NotEmpty().WithMessage("تکرار رمز عبور جدید الزامی است.")
            .MinimumLength(8).WithMessage("تکرار رمز عبور جدید باید حداقل 8 کاراکتر باشد.")
            .MaximumLength(70).WithMessage("تکرار رمز عبور جدید میتواند حداکثر 70 کاراکتر باشد.")
            .Matches(@"^[a-zA-Z0-9_@!-]+$")
            .WithMessage("تکرار رمز عبور جدید فقط می‌تواند شامل حروف، اعداد یا _ ,@ ,! ,- ,باشد.");

        RuleFor(x => x)
            .Must(x => x.NewPassword == x.NewPassswordRepeat)
            .WithMessage("رمز عبور جدید و تکرار آن باید یکسان باشد.");
    }
}