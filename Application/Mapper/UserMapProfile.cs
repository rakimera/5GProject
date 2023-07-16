using Application.Models;
using Application.Models.Users;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class UserMapProfile : Profile
{
    public UserMapProfile()
    {
        CreateMap<User, UserDto>()
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
                opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.Role, opt =>
                opt.MapFrom(src => src.Role)).ReverseMap();

        CreateMap<UserDto, UpdateUserDto>()
            .ForMember(dest => dest.Oid, opt =>
                opt.MapFrom(src => src.Oid))
            .ForMember(dest => dest.Name, opt =>
                opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Surname, opt =>
                opt.MapFrom(src => src.Surname))
            .ForMember(dest => dest.Role, opt =>
                opt.MapFrom(src => src.Role)).ReverseMap();
        
        CreateMap<UserDto, CreateUserDto>()
            .ForMember(dest => dest.Name, opt =>
                opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Surname, opt =>
                opt.MapFrom(src => src.Surname))
            .ForMember(dest => dest.Role, opt =>
                opt.MapFrom(src => src.Role)).ReverseMap();
    }
}