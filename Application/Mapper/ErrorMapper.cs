using AutoMapper;
using FluentValidation.Results;

namespace Application.Mapper;

public class ErrorMapper : Profile
{
    public ErrorMapper()
    {
        CreateMap<ValidationFailure, string>()
            .AfterMap((src, dest) => dest = src.ErrorMessage);
    }
}