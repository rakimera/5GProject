using Application.Models.TranslatorSpecs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class TranslatorSpecsProfile : Profile
{
    public TranslatorSpecsProfile()
    {
        CreateMap<TranslatorSpecs, TranslatorSpecsDto>()
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
            .ForMember(dest => dest.Frequency, opt =>
                opt.MapFrom(src => src.Frequency))
            .ForMember(dest => dest.AntennaId, opt =>
                opt.MapFrom(src => src.AntennaId))
            .ForMember(dest => dest.Antenna, opt =>
                opt.MapFrom(src => src.Antenna))
            .ForMember(dest => dest.RadiationZones, opt =>
                opt.MapFrom(src => src.RadiationZones))
            .ReverseMap();
        
        CreateMap<CreateTranslatorSpecsDto, TranslatorSpecsDto>()
            .ForMember(dest => dest.Frequency, opt =>
                opt.MapFrom(src => src.Frequency))
            .ForMember(dest => dest.AntennaId, opt =>
                opt.MapFrom(src => src.AntennaId))
            .ForMember(dest => dest.Antenna, opt =>
                opt.MapFrom(src => src.Antenna))
            .ForMember(dest => dest.RadiationZones, opt =>
                opt.MapFrom(src => src.RadiationZones))
            .ReverseMap();
        
        CreateMap<TranslatorSpecsDto, UpdateTranslatorSpecsDto>()
            .ForMember(dest => dest.Frequency, opt =>
                opt.MapFrom(src => src.Frequency))
            .ForMember(dest => dest.AntennaId, opt =>
                opt.MapFrom(src => src.AntennaId))
            .ForMember(dest => dest.Antenna, opt =>
                opt.MapFrom(src => src.Antenna))
            .ForMember(dest => dest.RadiationZones, opt =>
                opt.MapFrom(src => src.RadiationZones))
            .ReverseMap();
    }
}