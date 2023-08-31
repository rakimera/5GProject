using Application.Models.AntennaTranslator;
using FluentValidation;

namespace Application.Validation;

public class TranslatorTypeValidator : AbstractValidator<TranslatorTypeDto>
{
    public TranslatorTypeValidator()
    {
        RuleFor(dto => dto.Type)
            .NotNull().WithMessage("Тип передатчика не задан");
    }
}