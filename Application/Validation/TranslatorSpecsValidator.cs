using Application.Models.TranslatorSpecs;
using FluentValidation;

namespace Application.Validation;

public class TranslatorSpecsValidator : AbstractValidator<TranslatorSpecsDto>
{
    public TranslatorSpecsValidator()
    {
        RuleFor(dto => dto.Frequency).GreaterThan(0).WithMessage("Частота передатчика должна быть больше нуля");
    }
}