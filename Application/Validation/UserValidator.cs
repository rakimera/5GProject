using System.Text.RegularExpressions;
using Application.Models;
using FluentValidation;

namespace Application.Validation;

public class UserValidator : AbstractValidator<UserDTO>
{
    public UserValidator()
    {
        RuleFor(user => user.Name)
            .MinimumLength(1).WithMessage("Слишком короткое имя")
            .MaximumLength(50).WithMessage("Слишком длинное имя")
            .NotEmpty().WithMessage("Имя должно быть заполнено")
            .Must(name =>
            {
                string pattern = @"^[а-яА-Я]+$";
                if (Regex.IsMatch(name, pattern))
                    return true;
                return false;
            }).WithMessage("Неверный формат имени");
        RuleFor(user => user.Surname)
            .MinimumLength(1).WithMessage("Слишком короткая фамилия")
            .MaximumLength(50).WithMessage("Слишком длинная фамилия")
            .NotEmpty().WithMessage("Фамилия должна быть заполнена")
            .Must(name =>
            {
                string pattern = @"^[а-яА-Я]+$";
                if (Regex.IsMatch(name, pattern))
                    return true;
                return false;
            }).WithMessage("Неверный формат фамилии");
    }
}