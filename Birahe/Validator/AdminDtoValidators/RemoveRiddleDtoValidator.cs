using Birahe.EndPoint.Models.Dto;
using Birahe.EndPoint.Models.Dto.AdminDto_s;
using FluentValidation;

namespace Birahe.EndPoint.Validator;

public class RemoveRiddleDtoValidator : AbstractValidator<RemoveRiddleDto> {
    public RemoveRiddleDtoValidator() {
        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("شعبه معما نمیتواند خالی باشد.")
            .MinimumLength(4).WithMessage("شعبه معما باید حداقل ۴ کاراکتر باشد.")
            .MaximumLength(200).WithMessage("شعبه معما میتواند حداکثر 200 کاراکتر باشد.")
            .Matches(@"^[a-zA-Z0-9]+$")
            .WithMessage("شعبه معما فقط می‌تواند شامل حروف انگلیسی باشد.");

        RuleFor(x => x.No)
            .NotEmpty().WithMessage("شماره معما نمیتواند خالی باشد.")
            .Must(l => l > 0 && l < 16).WithMessage("شماره معما باید بزرگتر از 0 و کوچکتر از 16 باشد.");
    }
}