using Application.Models.Projects;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class ProjectMapProfile : Profile
{
    public ProjectMapProfile()
    {
        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.ContrAgentId, opt =>
                opt.MapFrom(src => src.ContrAgentOid))
            .ForMember(dest => dest.ContrAgent, opt =>
                opt.MapFrom(src => src.ContrAgent))
            .ForMember(dest => dest.ExecutorId, opt =>
                opt.MapFrom(src => src.ExecutorOid))
            .ForMember(dest => dest.Executor, opt =>
                opt.MapFrom(src => src.Executor))
            .ForMember(dest => dest.ProjectStatusId, opt =>
                opt.MapFrom(src => src.ProjectStatusOid))
            .ForMember(dest => dest.ProjectStatus, opt =>
                opt.MapFrom(src => src.ProjectStatus))
            .ForMember(dest => dest.DistrictId, opt =>
                opt.MapFrom(src => src.DistrictOid))
            .ForMember(dest => dest.District, opt =>
                opt.MapFrom(src => src.District))
            .ForMember(dest => dest.TownId, opt =>
                opt.MapFrom(src => src.TownOid))
            .ForMember(dest => dest.Town, opt =>
                opt.MapFrom(src => src.Town))
            .ForMember(dest => dest.ProjectAntennae, opt =>
                opt.MapFrom(src => src.ProjectAntennae))
            .ForMember(dest => dest.Street, opt =>
                opt.MapFrom(src => src.Street))
            .ForMember(dest => dest.House, opt =>
                opt.MapFrom(src => src.House)).ReverseMap();

    }
}