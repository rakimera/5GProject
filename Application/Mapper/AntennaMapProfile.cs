using Application.Models.Antennae;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class AntennaMapProfile : Profile
{
    public AntennaMapProfile()
    {
        CreateMap<Antenna, AntennaDto>()
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
            .ForMember(dest => dest.Model, opt =>
                opt.MapFrom(src => src.Model))
            .ForMember(dest => dest.TranslatorSpecsList, opt =>
                opt.MapFrom(src => src.TranslatorSpecsList))
            .ForMember(dest => dest.VerticalSizeDiameter, opt =>
                opt.MapFrom(src => src.VerticalSizeDiameter))
            .ReverseMap();
        
        CreateMap<CreateAntennaDto, AntennaDto>()
            .ForMember(dest => dest.Model, opt =>
                opt.MapFrom(src => src.Model))
            .ForMember(dest => dest.VerticalSizeDiameter, opt =>
                opt.MapFrom(src => src.VerticalSizeDiameter))
            .ReverseMap();
        
        CreateMap<AntennaDto, UpdateAntennaDto>()
            .ForMember(dest => dest.Model, opt =>
                opt.MapFrom(src => src.Model))
            .ForMember(dest => dest.VerticalSizeDiameter, opt =>
                opt.MapFrom(src => src.VerticalSizeDiameter))
            .ReverseMap();
    }
}