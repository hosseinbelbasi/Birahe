using Birahe.EndPoint.Models.Dto.AdminDto_s;
using FluentValidation;

namespace Birahe.EndPoint.Validator.AdminDtoValidators;

public class AddRiddleDtoValidator : AbstractValidator<AddRiddleDto> {
    public AddRiddleDtoValidator() {
        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("شعبه معما نمیتواند خالی باشد.")
            .MinimumLength(4).WithMessage("شعبه معما باید حداقل ۴ کاراکتر باشد.")
            .MaximumLength(200).WithMessage("شعبه معما میتواند حداکثر 200 کاراکتر باشد.")
            .Matches(@"^[\u0600-\u06FF\s]+$")
            .WithMessage("شعبه معما فقط می‌تواند شامل حروف فارسی باشد.");

        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("سطح معما نمیتواند خالی باشد.")
            .Must(l => l > 0 && l <= 3).WithMessage("سطح معما باید بزرگتر از 0 و کوچکتر از 3 باشد.");

        RuleFor(x => x.No)
            .NotEmpty().WithMessage("شماره معما نمیتواند خالی باشد.")
            .Must(l => l > 0 && l < 16).WithMessage("شماره معما باید بزرگتر از 0 و کوچکتر از 16 باشد.");


        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("محتوای معما نمیتواند خالی باشد.")
            .MinimumLength(4).WithMessage("محتوای معما باید حداقل ۴ کاراکتر باشد.")
            .MaximumLength(1000).WithMessage("محتوای معما میتواند حداکثر 1000 کاراکتر باشد.")
            .Matches(@"^[a-zA-Z0-9_.!?\u0600-\u06FF\s]+$")
            .WithMessage("محتوای معما فقط می‌تواند شامل حروف، اعداد یا _ باشد.");


        RuleFor(x => x.Answer)
            .NotEmpty().WithMessage("جواب معما نمیتواند خالی باشد.")
            .MinimumLength(4).WithMessage("جواب معما باید حداقل ۴ کاراکتر باشد.")
            .MaximumLength(2000).WithMessage("جواب معما میتواند حداکثر 1000 کاراکتر باشد.");


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