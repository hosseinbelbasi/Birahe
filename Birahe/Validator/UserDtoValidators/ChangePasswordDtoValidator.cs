using Birahe.EndPoint.Models.Dto.UserDto_s;
using FluentValidation;

namespace Birahe.EndPoint.Validator.UserDtoValidators;

public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto> {
    public ChangePasswordDtoValidator() {


        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("رمز عبور قدیمی الزامی است.")
            .MinimumLength(8).WithMessage("رمز عبور قدیمی باید حداقل 8 کاراکتر باشد.")
            .MaximumLength(70).WithMessage("رمز عبور قدیمی میتواند حداکثر 70 کاراکتر باشد.")
            .Matches(@"^[a-zA-Z0-9_@!-]+$").WithMessage("رمز عبور قدیمی فقط می‌تواند شامل حروف، اعداد یا _ ,@ ,! ,- ,باشد.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("رمز عبور جدید الزامی است.")
            .MinimumLength(8).WithMessage("رمز عبور جدید باید حداقل 8 کاراکتر باشد.")
            .MaximumLength(70).WithMessage("رمز عبور جدید میتواند حداکثر 70 کاراکتر باشد.")
            .Matches(@"^[a-zA-Z0-9_@!-]+$").WithMessage("رمز عبور جدید فقط می‌تواند شامل حروف، اعداد یا _ ,@ ,! ,- ,باشد.");

        RuleFor(x => x.NewPasswordRepeated)
            .NotEmpty().WithMessage("تکرار رمز عبور جدید الزامی است.")
            .MinimumLength(8).WithMessage("تکرار رمز عبور جدید باید حداقل 8 کاراکتر باشد.")
            .MaximumLength(70).WithMessage("تکرار رمز عبور جدید میتواند حداکثر 70 کاراکتر باشد.")
            .Matches(@"^[a-zA-Z0-9_@!-]+$").WithMessage("تکرار رمز عبور جدید فقط می‌تواند شامل حروف، اعداد یا _ ,@ ,! ,- ,باشد.");

        RuleFor(x => x)
            .Must(x => x.NewPassword == x.NewPasswordRepeated)
            .WithMessage("رمز عبور جدید و تکرار آن باید یکسان باشد.");
    }
}