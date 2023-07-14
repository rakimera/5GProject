using Application.Models.Users;
using FluentValidation;

namespace Application.Validation;

public class UserValidator : AbstractValidator<UserDTO>
{
    public UserValidator()
    {
        RuleFor(user => user.Name)
            .MinimumLength(2).WithMessage("Слишком короткое имя")
            .MaximumLength(50).WithMessage("Слишком длинное имя")
            .NotEmpty().WithMessage("Имя должно быть заполнено");
        RuleFor(user => user.Surname)
            .MinimumLength(2).WithMessage("Слишком короткая фамилия")
            .MaximumLength(50).WithMessage("Слишком длинная фамилия")
            .NotEmpty().WithMessage("Фамилия должна быть заполнена");
    }
}