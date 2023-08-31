using Application.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class DistrictMapProfile : Profile
{
    public DistrictMapProfile()
    {
        CreateMap<District, DistrictDto>()
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
            .ForMember(dest => dest.DistrictName, opt =>
                opt.MapFrom(src => src.DistrictName))
            .ForMember(dest => dest.Towns, opt =>
                opt.MapFrom(src => src.Towns))
            .ReverseMap();
    }
}