using Application.Models.RadiationZone;
using FluentValidation;

namespace Application.Validation;

public class RadiationZoneValidator : AbstractValidator<RadiationZoneDto>
{
    public RadiationZoneValidator()
    {
        RuleFor(dto => dto.Degree)
            .NotNull().WithMessage("Градус излучения радиоволн не задан");
        RuleFor(dto => dto.Value)
            .NotNull().WithMessage("Значение радиоволны не задано");
        RuleFor(dto => dto.DirectionType)
            .NotNull().WithMessage("Тип радиоволны не задан");
        RuleFor(dto => dto.TranslatorSpecsId)
            .NotNull().WithMessage("Передатчик к которому относится раадиоволна не задан");
    }
}