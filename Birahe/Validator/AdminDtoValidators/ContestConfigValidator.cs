using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Models.Dto.AdminDto_s;
using FluentValidation;

namespace Birahe.EndPoint.Validator.AdminDtoValidators;

public class ContestConfigValidator : AbstractValidator<SetContesConfigDto> {
    public ContestConfigValidator() {
        RuleFor(cc => cc)
            .Must(cc => cc.Key == "Contest" && cc.Key == "FinalContest");

        RuleFor(cc => cc)
            .Must(cc => DateTime.Compare(cc.StartTime, cc.EndTime) < 0)
            .WithMessage("تاریخ شروع باید قبل از تاریخ پایان باشد");
    }
}