using Application.Models.ContrAgents;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class ContrAgentMapProfile : Profile
{
    public ContrAgentMapProfile()
    {
        CreateMap<CounterAgent, ContrAgentDto>()
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
            .ForMember(dest => dest.AmplificationFactor, opt =>
                opt.MapFrom(src => src.TransmitLossFactor))
            .ForMember(dest => dest.CompanyName, opt =>
                opt.MapFrom(src => src.CompanyName))
            .ForMember(dest => dest.DirectorName, opt =>
                opt.MapFrom(src => src.DirectorName))
            .ForMember(dest => dest.DirectorPatronymic, opt =>
                opt.MapFrom(src => src.DirectorPatronymic))
            .ForMember(dest => dest.DirectorSurname, opt =>
                opt.MapFrom(src => src.DirectorSurname))
            .ForMember(dest => dest.BIN, opt =>
                opt.MapFrom(src => src.BIN))
            .ReverseMap();
        
        CreateMap<ContrAgentDto, UpdateContrAgentDto>()
            .ForMember(dest => dest.Id, opt =>
                opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.AmplificationFactor, opt =>
                opt.MapFrom(src => src.AmplificationFactor))
            .ForMember(dest => dest.CompanyName, opt =>
                opt.MapFrom(src => src.CompanyName))
            .ForMember(dest => dest.DirectorName, opt =>
                opt.MapFrom(src => src.DirectorName))
            .ForMember(dest => dest.DirectorPatronymic, opt =>
                opt.MapFrom(src => src.DirectorPatronymic))
            .ForMember(dest => dest.DirectorSurname, opt =>
                opt.MapFrom(src => src.DirectorSurname))
            .ForMember(dest => dest.BIN, opt =>
                opt.MapFrom(src => src.BIN)).ReverseMap();
        
        
        CreateMap<ContrAgentDto, CreateContrAgentDto>()
            .ForMember(dest => dest.AmplificationFactor, opt =>
                opt.MapFrom(src => src.AmplificationFactor))
            .ForMember(dest => dest.CompanyName, opt =>
                opt.MapFrom(src => src.CompanyName))
            .ForMember(dest => dest.DirectorName, opt =>
                opt.MapFrom(src => src.DirectorName))
            .ForMember(dest => dest.DirectorPatronymic, opt =>
                opt.MapFrom(src => src.DirectorPatronymic))
            .ForMember(dest => dest.DirectorSurname, opt =>
                opt.MapFrom(src => src.DirectorSurname))
            .ForMember(dest => dest.BIN, opt =>
                opt.MapFrom(src => src.BIN)).ReverseMap();
        
    }
}