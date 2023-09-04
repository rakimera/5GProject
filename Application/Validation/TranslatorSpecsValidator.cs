using Application.Models.TranslatorSpecs;
using FluentValidation;

namespace Application.Validation;

public class TranslatorSpecsValidator : AbstractValidator<TranslatorSpecsDto>
{
    public TranslatorSpecsValidator()
    {
        RuleFor(dto => dto.Frequency)
            .NotNull().WithMessage("Частота передатчика не задана");
        RuleFor(dto => dto.Gain)
            .NotNull().WithMessage("Коэффициент усиления передатчика не задан");
    }
}