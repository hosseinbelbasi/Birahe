using Birahe.EndPoint.Models.Dto.ContestDto_s;
using FluentValidation;

namespace Birahe.EndPoint.Validator.ContestDtoValidators;

public class SubmitAnswerDtoValidation : AbstractValidator<SubmitAnswerDto> {
    public SubmitAnswerDtoValidation() {
        RuleFor(ans => ans.Answer)
            .MaximumLength(2000).WithMessage("جواب معما حداکثر میتواند 2000 کاراکتر باشد");
    }
}