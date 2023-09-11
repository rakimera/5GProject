using Application.Models.RadiationZone.RadiationZoneExelFile;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class RadiationZoneExelFileMapProfile : Profile
{
    public RadiationZoneExelFileMapProfile()
    {
        CreateMap<RadiationZoneExelFile, RadiationZoneExelFileDto>()
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
            .ForMember(dest => dest.ExelFile, opt =>
                opt.MapFrom(src => src.ExelFile))
            .ForMember(dest => dest.TranslatorSpecId, opt =>
                opt.MapFrom(src => src.TranslatorSpecId))
            .ReverseMap();
    }
}