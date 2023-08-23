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
            .ForMember(dest => dest.ExecutiveCompany, opt =>
                opt.MapFrom(src => src.ExecutiveCompany))
            .ForMember(dest => dest.ExecutiveCompanyId, opt =>
                opt.MapFrom(src => src.ExecutiveCompanyId))
            .ForMember(dest => dest.DistrictName, opt =>
                opt.MapFrom(src => src.DistrictName))
            .ForMember(dest => dest.TownName, opt =>
                opt.MapFrom(src => src.TownName))
            .ForMember(dest => dest.Address, opt =>
                opt.MapFrom(src => src.Address));
        
        
        CreateMap<ProjectDto, Project>()
            .ForMember(dest => dest.ContrAgentId, opt =>
                opt.MapFrom(src => src.ContrAgentId))
            .ForMember(dest => dest.ExecutiveCompanyId, opt =>
                opt.MapFrom(src => src.ExecutiveCompanyId))
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
            .ForMember(dest => dest.Address, opt =>
                opt.MapFrom(src=> src.Address))
            .ForMember(dest => dest.TownName, opt =>
                opt.MapFrom(src=> src.TownName))
            .ForMember(dest => dest.DistrictName, opt =>
                opt.MapFrom(src=> src.DistrictName))
            .ForMember(dest => dest.ProjectAntennae, opt =>
                opt.Ignore())
            .ForMember(dest => dest.TotalFluxDensity, opt =>
                opt.Ignore())
            .ForMember(dest => dest.ExecutiveCompany, opt =>
                opt.Ignore());

        CreateMap<UpdateProjectDto, Project>()
            .ForMember(dest => dest.ContrAgentId, opt =>
                opt.MapFrom(src => src.ContrAgentId))
            .ForMember(dest => dest.ExecutorId, opt =>
                opt.MapFrom(src => src.ExecutorId))
            .ForMember(dest => dest.ProjectStatusId, opt =>
                opt.MapFrom(src => src.ProjectStatusId))
            .ForMember(dest => dest.ExecutiveCompanyId, opt =>
                opt.MapFrom(src => src.ExecutiveCompanyId))
            .ForMember(dest => dest.Address, opt =>
                opt.MapFrom(src=>src.Address))
            .ForMember(dest => dest.TownName, opt =>
                opt.MapFrom(src=>src.TownName))
            .ForMember(dest => dest.DistrictName, opt =>
                opt.MapFrom(src=>src.DistrictName));
        
        CreateMap<CreateProjectDto, ProjectDto>()
            .ForMember(dest => dest.ContrAgentId, opt =>
                opt.MapFrom(src => src.ContrAgentId))
            .ForMember(dest => dest.TownName, opt =>
                opt.MapFrom(src => src.TownName))
            .ForMember(dest => dest.Address, opt =>
                opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.ProjectNumber, opt =>
                opt.MapFrom(src => src.ProjectNumber));

    }
}