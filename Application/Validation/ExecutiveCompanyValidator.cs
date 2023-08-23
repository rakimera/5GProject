using Domain.Entities;
using FluentValidation;

namespace Application.Validation;

public class ExecutiveCompanyValidator : AbstractValidator<ExecutiveCompany>
{
    public ExecutiveCompanyValidator()
    {
        RuleFor(contrAgent => contrAgent.Address)
            .NotEmpty().WithMessage("Адрес компании должен быть заполнен");

        RuleFor(contrAgent => contrAgent.LicenseNumber)
            .NotEmpty().WithMessage("Поле лицензии компании должен быть заполнен");

        RuleFor(contrAgent => contrAgent.BIN)
            .NotEmpty().WithMessage("БИН компании должен быть заполнен");

        RuleFor(contrAgent => contrAgent.CompanyName)
            .NotEmpty().WithMessage("Название компании должно быть заполнено");
    }
}