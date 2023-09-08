using Application.Models.RadiationZone;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class RadiationZoneMapProfile : Profile
{
    public RadiationZoneMapProfile()
    {
        CreateMap<RadiationZone, RadiationZoneDto>()
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
            .ForMember(dest => dest.Degree, opt =>
                opt.MapFrom(src => src.Degree))
            .ForMember(dest => dest.Value, opt =>
                opt.MapFrom(src => src.Value))
            .ForMember(dest => dest.DirectionType, opt =>
                opt.MapFrom(src => src.DirectionType))
            .ForMember(dest => dest.TranslatorSpecsId, opt =>
                opt.MapFrom(src => src.TranslatorSpecsId))
            .ForMember(dest => dest.TranslatorSpecs, opt =>
                opt.MapFrom(src => src.TranslatorSpecs))
            .ReverseMap();
        
        CreateMap<CreateRadiationZoneDto, RadiationZoneDto>()
            .ForMember(dest => dest.Degree, opt =>
                opt.MapFrom(src => src.Degree))
            .ForMember(dest => dest.Value, opt =>
                opt.MapFrom(src => src.Value))
            .ForMember(dest => dest.DirectionType, opt =>
                opt.MapFrom(src => src.DirectionType))
            .ForMember(dest => dest.TranslatorSpecsId, opt =>
                opt.MapFrom(src => src.TranslatorSpecsId))
            .ForMember(dest => dest.TranslatorSpecs, opt =>
                opt.MapFrom(src => src.TranslatorSpecs))
            .ReverseMap();
        
        CreateMap<RadiationZoneDto, UpdateRadiationZoneDto>()
            .ForMember(dest => dest.Degree, opt =>
                opt.MapFrom(src => src.Degree))
            .ForMember(dest => dest.Value, opt =>
                opt.MapFrom(src => src.Value))
            .ForMember(dest => dest.DirectionType, opt =>
                opt.MapFrom(src => src.DirectionType))
            .ForMember(dest => dest.TranslatorSpecsId, opt =>
                opt.MapFrom(src => src.TranslatorSpecsId))
            .ForMember(dest => dest.TranslatorSpecs, opt =>
                opt.MapFrom(src => src.TranslatorSpecs))
            .ReverseMap();
    }
}