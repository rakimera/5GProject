using Domain.Entities;
using FluentValidation;

namespace Application.Validation;

public class CompanyLicenseValidator : AbstractValidator<CompanyLicense>
{
    public CompanyLicenseValidator()
    {
        RuleFor(dto => dto.Number)
            .NotNull().WithMessage("Номер не определен");
        RuleFor(dto => dto.DateOfIssue)
            .NotNull().WithMessage("Дата не задана");
    }
}