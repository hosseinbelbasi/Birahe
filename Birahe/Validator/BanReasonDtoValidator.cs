using Birahe.EndPoint.Models.Dto.AdminDto_s;
using FluentValidation;

namespace Birahe.EndPoint.Validator;

public class BanReasonDtoValidator : AbstractValidator<BanUserDto> {
    public BanReasonDtoValidator() {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("نام کاربری الزامی است.")
            .MinimumLength(4).WithMessage("نام کاربری باید حداقل ۴ کاراکتر باشد.")
            .MaximumLength(50).WithMessage("نام کاربری میتواند حداکثر 50 کاراکتر باشد.")
            .Matches(@"^[a-zA-Z0-9_]+$")
            .WithMessage("نام کاربری فقط می‌تواند شامل حروف، اعداد یا _ باشد.");

        RuleFor(x => x.BsnReason)
            .NotEmpty().WithMessage("علت بن شدن الزامی است.")
            .MinimumLength(10).WithMessage("علت بن شدن باید حداقل 10 کاراکتر باشد.")
            .MaximumLength(1000).WithMessage("علت بن شدن میتواند حداکثر 1000 کاراکتر باشد.")
            .Matches(@"^[a-zA-Z0-9_]+$")
            .WithMessage("علت بن شدن فقط می‌تواند شامل حروف، اعداد یا _ باشد.");

    }
}