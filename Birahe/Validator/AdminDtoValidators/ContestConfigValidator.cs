using Birahe.EndPoint.Entities;
using FluentValidation;

namespace Birahe.EndPoint.Validator.AdminDtoValidators;

public class ContestConfigValidator : AbstractValidator<ContestConfig> {
    public ContestConfigValidator() {
        RuleFor(cc => cc)
            .Must(cc => cc.Key == "Contest" && cc.Key == "FinalContest");
    }
}