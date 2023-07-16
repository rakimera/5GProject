using Application.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class UserMapProfile : Profile
{
    public UserMapProfile()
    {
        CreateMap<User, UserDto>()
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
                opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.Login, opt =>
                opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.Password, opt =>
                opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Role, opt =>
                opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.RefreshToken, opt =>
                opt.MapFrom(src => src.RefreshToken))
            .ForMember(dest => dest.RefreshTokenExpiryTime, opt =>
                opt.MapFrom(src => src.RefreshTokenExpiryTime)).ReverseMap();

        CreateMap<UserDto, UpdateUserDto>()
            .ForMember(dest => dest.Name, opt =>
                opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Surname, opt =>
                opt.MapFrom(src => src.Surname))
            .ForMember(dest => dest.Login, opt =>
                opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.Role, opt =>
                opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.Oid, opt =>
                opt.MapFrom(src => src.Oid)).ReverseMap();
        
        CreateMap<UserDto, CreateUserDto>()
            .ForMember(dest => dest.Name, opt =>
                opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Surname, opt =>
                opt.MapFrom(src => src.Surname))
            .ForMember(dest => dest.Login, opt =>
                opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.Password, opt =>
                opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Role, opt =>
                opt.MapFrom(src => src.Role)).ReverseMap();
    }
}