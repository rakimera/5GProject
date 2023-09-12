using Application.Models.AntennaTranslator;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class AntennaTranslatorMapProfile : Profile
{
    public AntennaTranslatorMapProfile()
    {
        CreateMap<AntennaTranslator, AntennaTranslatorDto>()
            .ForMember(dest => dest.Id, opt =>
                opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Created, opt =>
                opt.MapFrom(src => src.Created))
            .ForMember(dest => dest.LastModified, opt =>
                opt.MapFrom(src => src.LastModified))
            .ForMember(dest => dest.LastModifiedBy, opt =>
                opt.MapFrom(src => src.LastModifiedBy))
            .ForMember(dest => dest.CreatedBy, opt =>
                opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.ProjectAntenna, opt =>
                opt.MapFrom(src => src.ProjectAntenna))
            .ForMember(dest => dest.ProjectAntennaId, opt =>
                opt.MapFrom(src => src.ProjectAntennaId))
            .ForMember(dest => dest.TranslatorSpecs, opt =>
                opt.MapFrom(src => src.TranslatorSpecs))
            .ForMember(dest => dest.Power, opt =>
                opt.MapFrom(src => src.Power))
            .ForMember(dest => dest.TranslatorType, opt =>
                opt.MapFrom(src => src.TranslatorType))
            .ForMember(dest => dest.Gain, opt =>
                opt.MapFrom(src => src.Gain))
            .ForMember(dest => dest.Tilt, opt =>
                opt.MapFrom(src => src.Tilt))
            .ReverseMap();
    }
}