using Application.Models.EnergyResult;
using FluentValidation;

namespace Application.Validation;

public class EnergyResultValidator : AbstractValidator<CreateEnergyResultDto>
{
    public EnergyResultValidator()
    {
        
    }
}