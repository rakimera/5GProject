using Application.Models.TranslatorSpecs;
using FluentValidation;

namespace Application.Validation;

public class TranslatorSpecsValidator : AbstractValidator<TranslatorSpecsDto>
{
    public TranslatorSpecsValidator()
    {
        RuleFor(dto => dto.Frequency)
            .NotNull().WithMessage("Частота передатчика не задана");
    }
}