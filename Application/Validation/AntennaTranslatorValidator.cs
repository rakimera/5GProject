using Application.Models.AntennaTranslator;
using FluentValidation;

namespace Application.Validation;

public class AntennaTranslatorValidator : AbstractValidator<AntennaTranslatorDto>
{
    public AntennaTranslatorValidator()
    {
        RuleFor(dto => dto.TranslatorSpecsId)
            .NotNull().WithMessage("Частота передатчика не задана");
        RuleFor(dto => dto.Power)
            .NotNull().WithMessage("Мощность не задана");
        RuleFor(dto => dto.TransmitLossFactor)
            .NotNull().WithMessage("Коэффициент потери сигнала не задан");
        RuleFor(dto => dto.TranslatorTypeId)
            .NotNull().WithMessage("Тип передатчика не задан");
        RuleFor(dto => dto.Gain)
            .NotNull().WithMessage("Коэффициент усиления передатчика не задан");
        RuleFor(dto => dto.ProjectAntennaId)
            .NotNull().WithMessage("Проект не задан");
    }
}