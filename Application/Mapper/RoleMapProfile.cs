using Application.Models.Roles;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class RoleMapProfile : Profile
{
    public RoleMapProfile()
    {
        CreateMap<Role, RoleDto>()
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
            .ReverseMap();
        
        CreateMap<RoleDto, UpdateRoleDto>()
            .ForMember(dest => dest.Id, opt =>
                opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RoleName, opt =>
                opt.MapFrom(src => src.RoleName))
            .ReverseMap();
        
        CreateMap<RoleDto, CreateRoleDto>()
            .ForMember(dest => dest.RoleName, opt =>
                opt.MapFrom(src => src.RoleName))
            .ReverseMap();
    }
}