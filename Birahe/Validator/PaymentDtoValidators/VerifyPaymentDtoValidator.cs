using Birahe.EndPoint.Models.Dto.PaymentDto_s.Dto;
using FluentValidation;

namespace Birahe.EndPoint.Validator.PaymentDtoValidators;

public class VerifyPaymentDtoValidator : AbstractValidator<VerifyPaymentDto> {
    public VerifyPaymentDtoValidator() {
        RuleFor(dto => dto.Authority)
            .MaximumLength(40)
            .WithMessage("کد پرداخت میتواند حداکثر 40 کاراکتر باشد");
        RuleFor(dto => dto.Status)
            .Must(s => s == "OK" || s == "NOK")
            .WithMessage("کد وضعیت میتواند Ok یا NOK باشد");
    }
}