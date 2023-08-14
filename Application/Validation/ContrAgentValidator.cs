using Domain.Entities;
using FluentValidation;

namespace Application.Validation;

public class ContrAgentValidator : AbstractValidator<ContrAgent>
{
    public ContrAgentValidator()
    {
        RuleFor(contrAgent => contrAgent.CompanyName)
            .NotEmpty().WithMessage("Название компании должно быть заполнено");
        RuleFor(contrAgent => contrAgent.BIN)
            .NotEmpty().WithMessage("БИН компании должнен быть заполнен");
        RuleFor(contrAgent => contrAgent.TransmitLossFactor)
            .NotEmpty().WithMessage("Коэффициент усиления компании должнен быть заполнен");
        RuleFor(contrAgent => contrAgent.DirectorName)
            .NotEmpty().WithMessage("Имя директора компании должно быть заполнено");
        RuleFor(contrAgent => contrAgent.DirectorSurname)
            .NotEmpty().WithMessage("Фамилия директора компании должно быть заполнено");
    }
}