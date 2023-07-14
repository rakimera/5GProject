using AutoMapper;
using FluentValidation.Results;

namespace Application.Mapper;

public class ErrorMapper : Profile
{
    public ErrorMapper()
    {
        CreateMap<ValidationFailure, string>()
            .ForMember(dest => dest, opt =>
                opt.MapFrom(src => src.ErrorMessage));
    }
}