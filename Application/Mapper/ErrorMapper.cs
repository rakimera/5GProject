using AutoMapper;
using FluentValidation.Results;

namespace Application.Mapper;

public class ErrorMapper : Profile
{
    public ErrorMapper()
    {
        CreateMap<ValidationFailure, string>()
            .ConvertUsing(MapValidationFailureToString);
    }

    private string MapValidationFailureToString(ValidationFailure source, string destination, ResolutionContext context)
    {
        return source.ErrorMessage;
    }
}