using Application.Models.TranslatorSpecs;
using FluentValidation;

namespace Application.Validation;

public class TranslatorSpecsValidator : AbstractValidator<TranslatorSpecsDto>
{
    public TranslatorSpecsValidator()
    {
        RuleFor(dto => dto.Frequency)
            .NotNull().WithMessage("Частота передатчика не задана");
        RuleFor(dto => dto.Power)
            .NotNull().WithMessage("Мощность передатчика не задана");
        RuleFor(dto => dto.Gain)
            .NotNull().WithMessage("Коэффициент усиления передатчика не задан");
        RuleFor(dto => dto.AntennaId)
            .NotNull().WithMessage("Антенна передатчика не определена");
        RuleFor(dto => dto.RadiationZones)
            .NotNull().WithMessage("Зона покрытия 360 градусов не задана");
    }
}