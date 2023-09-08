using Application.Models.Users;
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
            .ForMember(dest => dest.ExecutiveCompanyId, opt =>
                opt.MapFrom(src => src.ExecutiveCompanyId))
            .ForMember(dest => dest.PhoneNumber, opt =>
                opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Patronymic, opt =>
                opt.MapFrom(src => src.Patronymic))
            .ReverseMap();
        
        CreateMap<UserDto, UpdateUserDto>()
            .ForMember(dest => dest.Id, opt =>
                opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt =>
                opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Surname, opt =>
                opt.MapFrom(src => src.Surname))
            .ForMember(dest => dest.Login, opt =>
                opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.Roles, opt =>
                opt.MapFrom(src => src.Roles))
            .ForMember(dest => dest.ExecutiveCompanyId, opt =>
                opt.MapFrom(src => src.ExecutiveCompanyId))
            .ForMember(dest => dest.PhoneNumber, opt =>
                opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Patronymic, opt =>
                opt.MapFrom(src => src.Patronymic))
            .ReverseMap();
        
        CreateMap<UserDto, CreateUserDto>()
            .ForMember(dest => dest.Name, opt =>
                opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Surname, opt =>
                opt.MapFrom(src => src.Surname))
            .ForMember(dest => dest.Login, opt =>
                opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.Password, opt =>
                opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Roles, opt =>
                opt.MapFrom(src => src.Roles))
            .ForMember(dest => dest.ExecutiveCompanyId, opt =>
                opt.MapFrom(src => src.ExecutiveCompanyId))
            .ForMember(dest => dest.PhoneNumber, opt =>
                opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Patronymic, opt =>
                opt.MapFrom(src => src.Patronymic))
            .ReverseMap();
    }
}