using Application.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class TownMapProfile : Profile
{
    public TownMapProfile()
    {
        CreateMap<Town, TownDto>()
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
            .ForMember(dest => dest.TownName, opt =>
                opt.MapFrom(src => src.TownName))
            .ForMember(dest => dest.DistrictId, opt =>
                opt.MapFrom(src => src.DistrictId))
            .ForMember(dest => dest.District, opt =>
                opt.MapFrom(src => src.District))
            .ReverseMap();
    }
}