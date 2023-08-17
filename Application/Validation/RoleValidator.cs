using Domain.Entities;
using FluentValidation;

namespace Application.Validation;

public class RoleValidator: AbstractValidator<Role>
{
    public RoleValidator()
    {
        RuleFor(dto => dto.RoleName)
            .MinimumLength(2).WithMessage("Слишком короткое название")
            .NotEmpty().WithMessage("Введите название роли");
    }
}