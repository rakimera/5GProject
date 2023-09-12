using Application.Models.Antennae;
using Application.Models.Projects.ProjectAntennas;
using FluentValidation;

namespace Application.Validation;

public class ProjectAntennaValidator : AbstractValidator<ProjectAntennaDto>
{
    public ProjectAntennaValidator()
    {
        RuleFor(dto => dto.Azimuth)
            .NotNull().WithMessage("Азимут антенны не записан");
        RuleFor(dto => dto.Height)
            .NotNull().WithMessage("Высота антенны не записана");
        RuleFor(dto => dto.Latitude)
            .NotNull().WithMessage("Широта установки антенны не записана");        
        RuleFor(dto => dto.Longitude)
            .NotNull().WithMessage("Долгота установки антенны не записана");
        RuleFor(dto => dto.Tilt)    
            .NotNull().WithMessage("Угол наклона антенны не записана"); 
        RuleFor(dto => dto.AntennaId)    
            .NotNull().WithMessage("Антенна не выбрана");
        RuleFor(dto => dto.RtoRadiationMode)    
            .NotEmpty().WithMessage("Режим работы РТО на излучение не записан");
    }
}