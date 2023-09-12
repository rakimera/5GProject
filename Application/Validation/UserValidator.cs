using Application.Models.Users;
using FluentValidation;

namespace Application.Validation;

public class UserValidator : AbstractValidator<UserDto>
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
        RuleFor(user => user.Login)
            .MinimumLength(9).WithMessage("Слишком короткий логин")
            .MaximumLength(100).WithMessage("Слишком длинный логин")
            .NotEmpty().WithMessage("Логин должнен быть заполнен")
            .EmailAddress().WithMessage("Введен некорректный логин");
        RuleFor(user => user.Password).NotEmpty().WithMessage("Пароль не может быть пустым.")
            .MinimumLength(8).WithMessage("Пароль не должен быть меньше 8 символов.")
            .MaximumLength(30).WithMessage("Пароль не должен быть больше 30 символов.")
            .Matches(@"[A-Z]+").WithMessage("В пароле должнен присутствовать минимум один символ в верхнем регистре.")
            .Matches(@"[a-z]+").WithMessage("В пароле должнен присутствовать минимум один символ в нижнем регистре.")
            .Matches(@"[0-9]+").WithMessage("В пароле должна присутствовать минимум одна цифра.")
            .Matches(@"[\!\?\*\.]*$").WithMessage("В пароле должнен присутствовать минимум один спецсимвол (!? *.)");
    }
}