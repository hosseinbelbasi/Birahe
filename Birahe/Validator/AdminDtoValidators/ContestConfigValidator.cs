using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Models.Dto.AdminDto_s;
using FluentValidation;

namespace Birahe.EndPoint.Validator.AdminDtoValidators;

public class ContestConfigValidator : AbstractValidator<SetContestConfigDto> {
    public ContestConfigValidator() {
        RuleFor(cc => cc)
            .Must(cc => AllowedConfigs.Any(ac=> String.Equals(ac, cc.Key)));

        RuleFor(cc => cc)
            .Must(cc => DateTime.Compare(cc.StartTime, cc.EndTime) < 0)
            .WithMessage("تاریخ شروع باید قبل از تاریخ پایان باشد");
    }

    private List<String> AllowedConfigs => new List<string>() {
        "Contest",
        "FinalContest",
        "Signup"
    };
}