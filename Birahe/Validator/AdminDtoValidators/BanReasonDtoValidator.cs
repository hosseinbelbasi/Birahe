using Birahe.EndPoint.Models.Dto.AdminDto_s;
using FluentValidation;

namespace Birahe.EndPoint.Validator;

public class BanReasonDtoValidator : AbstractValidator<BanUserDto> {
    public BanReasonDtoValidator() {


        RuleFor(x => x.BsnReason)
            .NotEmpty().WithMessage("علت بن شدن الزامی است.")
            .MinimumLength(10).WithMessage("علت بن شدن باید حداقل 10 کاراکتر باشد.")
            .MaximumLength(1000).WithMessage("علت بن شدن میتواند حداکثر 1000 کاراکتر باشد.")
            .Matches(@"^[a-zA-Z0-9_]+$")
            .WithMessage("علت بن شدن فقط می‌تواند شامل حروف، اعداد یا _ باشد.");
    }
}