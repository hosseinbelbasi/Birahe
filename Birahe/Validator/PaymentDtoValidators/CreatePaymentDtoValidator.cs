using Birahe.EndPoint.Models.Dto.PaymentDto_s.Dto;
using FluentValidation;

namespace Birahe.EndPoint.Validator.PaymentDtoValidators;

public class CreatePaymentDtoValidator : AbstractValidator<CreatePaymentDto> {
    public CreatePaymentDtoValidator() {
        RuleFor(dto => dto.Discount)
            .MaximumLength(30)
            .WithMessage("کد تخفیف میتواند حداکثر 30 کاراکتر باشد");
    }
}