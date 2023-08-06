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
                opt.MapFrom(src => src.ContrAgentId))
            .ForMember(dest => dest.ContrAgent, opt =>
                opt.MapFrom(src => src.ContrAgent))
            .ForMember(dest => dest.ExecutorId, opt =>
                opt.MapFrom(src => src.ExecutorId))
            .ForMember(dest => dest.Executor, opt =>
                opt.MapFrom(src => src.Executor))
            .ForMember(dest => dest.ProjectStatusId, opt =>
                opt.MapFrom(src => src.ProjectStatusId))
            .ForMember(dest => dest.ProjectStatus, opt =>
                opt.MapFrom(src => src.ProjectStatus))
            .ForMember(dest => dest.DistrictId, opt =>
                opt.MapFrom(src => src.DistrictId))
            .ForMember(dest => dest.District, opt =>
                opt.MapFrom(src => src.District))
            .ForMember(dest => dest.TownId, opt =>
                opt.MapFrom(src => src.TownId))
            .ForMember(dest => dest.Town, opt =>
                opt.MapFrom(src => src.Town))
            .ForMember(dest => dest.ProjectAntennae, opt =>
                opt.MapFrom(src => src.ProjectAntennae))
            .ForMember(dest => dest.Street, opt =>
                opt.MapFrom(src => src.Street))
            .ForMember(dest => dest.House, opt =>
                opt.MapFrom(src => src.House));
        
        CreateMap<ProjectDto, Project>()
            .ForMember(dest => dest.ContrAgentId, opt =>
                opt.MapFrom(src => src.ContrAgentId))
            .ForMember(dest => dest.ContrAgent, opt =>
                opt.Ignore())
            .ForMember(dest => dest.ExecutorId, opt =>
                opt.MapFrom(src => src.ExecutorId))
            .ForMember(dest => dest.Executor, opt =>
                opt.Ignore())
            .ForMember(dest => dest.ProjectStatusId, opt =>
                opt.MapFrom(src => src.ProjectStatusId))
            .ForMember(dest => dest.ProjectStatus, opt =>
                opt.Ignore())
            .ForMember(dest => dest.DistrictId, opt =>
                opt.MapFrom(src => src.DistrictId))
            .ForMember(dest => dest.District, opt =>
                opt.Ignore())
            .ForMember(dest => dest.TownId, opt =>
                opt.MapFrom(src => src.TownId))
            .ForMember(dest => dest.Town, opt =>
                opt.Ignore())
            .ForMember(dest => dest.ProjectAntennae, opt =>
                opt.Ignore())
            .ForMember(dest => dest.Street, opt =>
                opt.MapFrom(src => src.Street))
            .ForMember(dest => dest.House, opt =>
                opt.MapFrom(src => src.House));
        
        CreateMap<CreateProjectDto, ProjectDto>()
            .ForMember(dest => dest.ContrAgentId, opt =>
                opt.MapFrom(src => src.ContrAgentId))
            .ForMember(dest => dest.DistrictId, opt =>
                opt.MapFrom(src => src.DistrictId))
            .ForMember(dest => dest.TownId, opt =>
                opt.MapFrom(src => src.TownId))
            .ForMember(dest => dest.Street, opt =>
                opt.MapFrom(src => src.Street))
            .ForMember(dest => dest.House, opt =>
                opt.MapFrom(src => src.House)).ReverseMap();

    }
}