using Birahe.EndPoint.Models.Dto;
using Birahe.EndPoint.Models.Dto.AdminDto_s;
using FluentValidation;

namespace Birahe.EndPoint.Validator;

public class AdminEditUserUsernameValidator : AbstractValidator<AdminEditUserUsernameDto> {
    public AdminEditUserUsernameValidator() {

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


    }
}