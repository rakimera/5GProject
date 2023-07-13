using Application.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class UserMapProfile : Profile
{
    public UserMapProfile()
    {
        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.Id, opt =>
                opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Oid, opt =>
                opt.MapFrom(src => src.Oid))
            .ForMember(dest => dest.Name, opt =>
                opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Surname, opt =>
                opt.MapFrom(src => src.Surname))
            .ForMember(dest => dest.Created, opt =>
                opt.MapFrom(src => src.Created))
            .ForMember(dest => dest.LastModified, opt =>
                opt.MapFrom(src => src.LastModified))
            .ForMember(dest => dest.LastModifiedBy, opt =>
                opt.MapFrom(src => src.LastModifiedBy))
            .ForMember(dest => dest.CreatedBy, opt =>
                opt.MapFrom(src => src.CreatedBy)).ReverseMap();

        CreateMap<UserDTO, UpdateUserDto>()
            .ForMember(dest => dest.Id, opt =>
                opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt =>
                opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Surname, opt =>
                opt.MapFrom(src => src.Surname)).ReverseMap();
        
        CreateMap<UserDTO, CreateUserDto>()
            .ForMember(dest => dest.Name, opt =>
                opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Surname, opt =>
                opt.MapFrom(src => src.Surname)).ReverseMap();
    }
}