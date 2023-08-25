using Application.Models.Antennae;
using Application.Models.Projects.ProjectAntennas;
using FluentValidation;

namespace Application.Validation;

public class ProjectAntennaValidator : AbstractValidator<ProjectAntennaDto>
{
    public ProjectAntennaValidator()
    {
        
    }
}