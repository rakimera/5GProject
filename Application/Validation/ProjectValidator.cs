using Application.Models.Projects;
using FluentValidation;

namespace Application.Validation;

public class ProjectValidator : AbstractValidator<ProjectDto>
{
    public ProjectValidator()
    {
        RuleFor(dto => dto.ContrAgentId)
            .NotNull().WithMessage("Контрагент не выбран");
        RuleFor(dto => dto.ExecutorId)
            .NotNull().WithMessage("Ответственный за проект не выбран");
        RuleFor(dto => dto.ProjectStatusId)
            .NotNull().WithMessage("Статус проекта не выбран");
        RuleFor(dto => dto.DistrictId)
            .NotNull().WithMessage("Область установки не выбрана");
        RuleFor(dto => dto.TownId)
            .NotNull().WithMessage("Город установки не выбран");
    }
}