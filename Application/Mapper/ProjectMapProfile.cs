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
            .ForMember(dest => dest.TownName, opt =>
                opt.MapFrom(src => src.Address.Split(',', StringSplitOptions.TrimEntries)[0]))
            .ForMember(dest => dest.Arial, opt =>
                opt.MapFrom(src => src.Address.Split(',', StringSplitOptions.TrimEntries)[1]))
            .ForMember(dest => dest.Street, opt =>
                opt.MapFrom(src => src.Address.Split(',', StringSplitOptions.TrimEntries)[2]))
            .ForMember(dest => dest.House, opt =>
                opt.MapFrom(src => src.Address.Split(',', StringSplitOptions.TrimEntries)[3]));
        
        
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
                opt.MapFrom(src => $"{src.TownName+", "}{src.Arial+", "}{src.Street+", "}{src.House}"))
            .ForMember(dest => dest.ProjectAntennae, opt =>
                opt.Ignore())
            .ForMember(dest => dest.TotalFluxDensity, opt =>
                opt.Ignore())
            .ForMember(dest => dest.ExecutiveCompany, opt =>
                opt.Ignore());
        
        CreateMap<CreateProjectDto, ProjectDto>()
            .ForMember(dest => dest.ContrAgentId, opt =>
                opt.MapFrom(src => src.ContrAgentId))
            .ForMember(dest => dest.Arial, opt =>
                opt.MapFrom(src => src.Arial))
            .ForMember(dest => dest.TownName, opt =>
                opt.MapFrom(src => src.TownName))
            .ForMember(dest => dest.House, opt =>
                opt.MapFrom(src => src.House))
            .ForMember(dest => dest.Street, opt =>
                opt.MapFrom(src => src.Street))
            .ForMember(dest => dest.ProjectNumber, opt =>
                opt.MapFrom(src => src.ProjectNumber));
        
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
                opt.MapFrom(src => $"{src.TownName+", "}{src.Arial+", "}{src.Street+", "}{src.House}"));

    }
}