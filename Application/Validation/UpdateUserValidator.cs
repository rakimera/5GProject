using System.Text.RegularExpressions;
using Application.Models.Users;
using FluentValidation;

namespace Application.Validation;

public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator()
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
        RuleFor(user => user.Password).Must((pass) =>
        {
            if (!string.IsNullOrEmpty(pass))
            {
                string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,30}$";
                if (Regex.IsMatch(pass, passwordPattern)) 
                    return true;
                
                return false;
            }
            return true;
        }).WithMessage("Пароль не соответсвует требования.");
    }
}