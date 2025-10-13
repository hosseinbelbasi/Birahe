using Birahe.EndPoint.Models.Dto.ContestDto_s;
using FluentValidation;

namespace Birahe.EndPoint.Validator.ContestDtoValidators;

public class ContestRiddleDtoValidator : AbstractValidator<ContestRiddleDto> {
    public ContestRiddleDtoValidator() {
        RuleFor(x=> x.Department)
            .NotEmpty().WithMessage("شعبه معما نمیتواند خالی باشد.")
            .MinimumLength(4).WithMessage("شعبه معما باید حداقل ۴ کاراکتر باشد.")
            .MaximumLength(200).WithMessage("شعبه معما میتواند حداکثر 200 کاراکتر باشد.")
            .Matches(@"^[a-zA-Z0-9]+$")
            .WithMessage("شعبه معما فقط می‌تواند شامل حروف، اعداد باشد.");

        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("سطح معما نمیتواند خالی باشد.")
            .Must(l => l > 0 && l < 3).WithMessage("سطح معما باید بزرگتر از 0 و کوچکتر از 3 باشد.");

        RuleFor(x => x.No)
            .NotEmpty().WithMessage("شماره معما نمیتواند خالی باشد.")
            .Must(l => l > 0 && l < 16).WithMessage("شماره معما باید بزرگتر از 0 و کوچکتر از 16 باشد.");

        RuleFor(x => x.OpeningCost)
            .NotEmpty().WithMessage("قیمت باز کردن معما نمیتواند خالی باشد.")
            .Must(l => l > 0).WithMessage("قیمت باز کردن معما باید بزرگتر از 0  باشد.");

        RuleFor(x => x.HintCost)
            .NotEmpty().WithMessage("قیمت نکته معما نمیتواند خالی باشد.")
            .Must(l => l > 0).WithMessage("قیمت نکته معما باید بزرگتر از 0  باشد.");

        RuleFor(x => x.Reward)
            .NotEmpty().WithMessage("پاداش معما نمیتواند خالی باشد.")
            .Must(l => l > 0).WithMessage("پاداش معما باید بزرگتر از 0  باشد.");
    }
}