using Application.Models.Antennae;
using FluentValidation;

namespace Application.Validation;

public class ProjectAntennaValidator : AbstractValidator<ProjectAntennaDto>
{
    public ProjectAntennaValidator()
    {
        
    }
}